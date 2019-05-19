using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMagic : MonoBehaviour
{
    public class DrawEvent : UnityEvent<int> { }
    public DrawEvent onPlayerDraw = new DrawEvent();

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
            DrawOnTexture.Instance.Draw(position, out pixelCleared);

            onPlayerDraw.Invoke(pixelCleared);
        }
    }
}
