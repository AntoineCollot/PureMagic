using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawOnTexture : MonoBehaviour
{
    RenderTexture rt;
    public Texture2D brush;
    public int brushSize;
    Texture2D tex;

    public Renderer worms;
    Material mat;


    [HideInInspector]
    public int deltaPixelDrawn;
    int lastRemaining;
    public int Remaining
    {
        get
        {
            int pixelRemaining = 0;

            Color[] pixels = tex.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].r > 0.1f)
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
        Graphics.Blit(paintMap, rt);
        tex = paintMap;

        UpdateMap();

        lastRemaining = Remaining;

        Debug.Log("Pixels to clear :" + lastRemaining);
    }

    public void UpdateMap()
    {
        mat.SetTexture("_PaintMap", tex);
    }

    public void Draw(Vector2 pos, out int pixelCleared)
    {
        tex = new Texture2D(rt.width, rt.height);
        //Debug.Log(rt.width + " / " + rt.height);
        //Debug.Log(Screen.width + " / " + Screen.height);
        //Debug.Log(Camera.main.pixelWidth + " / " + Camera.main.pixelHeight);
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, rt.width, rt.height, 0);
        Graphics.DrawTexture(new Rect(pos.x* rt.width - brushSize * 0.5f,  (1-pos.y) * rt.height - brushSize * 0.5f, brushSize, brushSize), brush);// draw the brush on PaintMap
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply(false);
        GL.PopMatrix();
        RenderTexture.active = null;

        //Update stats
        int pixelRemaining = Remaining;
        deltaPixelDrawn = lastRemaining- pixelRemaining;
        lastRemaining = pixelRemaining;

        pixelCleared = deltaPixelDrawn;

        UpdateMap();
    }
}
