using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMagic : MonoBehaviour
{
    public class DrawEvent : UnityEvent<int> { }
    public DrawEvent onPlayerDraw = new DrawEvent();
    Vector2 lastDrawPosition;

    public static PlayerMagic Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            int pixelCleared = 0;
            Vector2 position = Input.mousePosition;
            position.x /= Screen.width;
            position.y /= Screen.height;

            //If it's the first frame we press the button, don't take into account the lastPosition saved
            if(Input.GetMouseButtonDown(0))
            {
                lastDrawPosition = position;
            }
            DrawOnTexture.Instance.Draw(position, lastDrawPosition, out pixelCleared);

            //Save the position we just drawed on for next frame
            lastDrawPosition = position;

            onPlayerDraw.Invoke(pixelCleared);
        }
    }
}
