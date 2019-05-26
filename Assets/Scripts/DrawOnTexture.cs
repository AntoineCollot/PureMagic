using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawOnTexture : MonoBehaviour
{
    RenderTexture rt;
    public Texture2D brush;
    public int brushSize;
    Texture2D currentPaintMap;

    public Renderer worms;
    Material mat;

    [HideInInspector]
    public int deltaPixelDrawn;
    int lastRemaining;

    //Do this in while cleaning to save performance instead
    public int Remaining
    {
        get
        {
            int pixelRemaining = 0;

            Color[] pixels = currentPaintMap.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].r > 0.1f) // or ==1 if using cleaning
                    pixelRemaining++;
            }

            return pixelRemaining;
        }
    }

    public static DrawOnTexture Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mat = worms.material;
        Texture2D paintMap = (Texture2D)mat.GetTexture("_PaintMap");
        rt = new RenderTexture(paintMap.width, paintMap.height, 32);
        //Make it writable so the clean image compute shader can write on it
        rt.enableRandomWrite = true;
        Graphics.Blit(paintMap, rt);
        this.currentPaintMap = paintMap;

        UpdateMap();

        lastRemaining = Remaining;

#if UNITY_EDITOR
        Debug.Log("Pixels to clear :" + lastRemaining);
#endif
    }

    public void UpdateMap()
    {
        mat.SetTexture("_PaintMap", currentPaintMap);
    }

    public void Draw(Vector2 currentPos, Vector2 lastPos, out int pixelCleared)
    {
        currentPaintMap = new Texture2D(rt.width, rt.height);

        //Find the draw position on the paint map
        Vector2 currentDrawPos = new Vector2(currentPos.x * rt.width - brushSize * 0.5f, (1 - currentPos.y) * rt.height - brushSize * 0.5f);
        Vector2 lastDrawPos = new Vector2(lastPos.x * rt.width - brushSize * 0.5f, (1 - lastPos.y) * rt.height - brushSize * 0.5f);

        //Get the positions to draw on
        List<Vector2> drawPositions = GetIntermediatePositions(currentDrawPos, lastDrawPos); 

        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, rt.width, rt.height, 0);

        //Draw on every positions
        for (int i = 0; i < drawPositions.Count; i++)
        {
            Graphics.DrawTexture(new Rect(drawPositions[i].x, drawPositions[i].y, brushSize, brushSize), brush);// draw the brush on PaintMap
        }

        currentPaintMap.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        currentPaintMap.Apply(false);
        GL.PopMatrix();
        RenderTexture.active = null;

        //Update stats
        int pixelRemaining = Remaining;
        deltaPixelDrawn = lastRemaining- pixelRemaining;
        lastRemaining = pixelRemaining;

        pixelCleared = deltaPixelDrawn;

        UpdateMap();

        //Clean the image using a compute shader
        CleanImage.CleanCompute(ref rt);
    }

    public List<Vector2> GetIntermediatePositions(Vector2 currentPos, Vector2 lastPos)
    {
        //Create a list holding all the positions to draw on
        List<Vector2> positions = new List<Vector2>();

        //Get the max distance before we add an intermediate postion
        float maxDistance = (float)brushSize / 2;

        //Find the number of pos to add in between
        int intermediatePosCount = Mathf.FloorToInt(Vector2.Distance(lastPos, currentPos) / maxDistance);
        for (int i = 1; i <= intermediatePosCount; i++)
        {
            //Add all the in between positions
            positions.Add(Vector2.Lerp(lastPos, currentPos, (float)i / (intermediatePosCount+1)));
        }

        //Always Add the current position
        positions.Add(currentPos);

        return positions;
    }
}
