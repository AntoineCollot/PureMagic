using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int width = 480;
        int height = 270;
        while(width*2<=Screen.width && height*2<=Screen.height)
        {
            width *= 2;
            height *= 2;
        }

        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}
