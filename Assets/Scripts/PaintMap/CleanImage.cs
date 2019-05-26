using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clean the lonely pixels using a compute shader (GPU)
/// </summary>
public static class CleanImage
{
    ////Too costly ~5 to 10 ms in profiler, 2 in real time
    //public static void Clean(ref Texture2D image, out int whitePixelsRemaining)
    //{
    //    //float time = Time.realtimeSinceStartup;
    //    //int pixelCleared = 0;

    //    whitePixelsRemaining = 0;
    //    float currentPixelValue;
    //    for (int y = 0; y < image.height; y++)
    //    {
    //        for (int x = 0; x < image.width; x++)
    //        {
    //            currentPixelValue = image.GetPixel(x, y).r;

    //            //Count the white pixels at the same time
    //            if (currentPixelValue == 1)
    //            {
    //                whitePixelsRemaining++;
    //            }
    //            //Check if it's a edge pixel (grey, so not white, not black)
    //            else if (currentPixelValue > 0)
    //            {
    //                bool foundWhite = false;
    //                //Check if any of its neighbourg is a white pixel (pixel with stuff on it)
    //                for (int yn = -1; yn <= 1; yn++)
    //                {
    //                    for (int xn = -1; xn <= 1; xn++)
    //                    {
    //                        //ignore ourself
    //                        if (xn == 0 && yn == 0)
    //                            continue;

    //                        if (image.GetPixel(x + xn, y + yn).r >= 1)
    //                        {
    //                            foundWhite = true;
    //                            break;
    //                        }
    //                    }

    //                    if (foundWhite)
    //                        break;
    //                }

    //                //Turn the pixel black if we didn't find any white nearby
    //                if (!foundWhite)
    //                {
    //                    image.SetPixel(x, y, Color.black);
    //                    //pixelCleared++;
    //                }

    //            }
    //        }
    //    }

    //    //image.Apply();
    //    //print("Cleared " + pixelCleared + " pixels in "+(Time.realtimeSinceStartup - time));
    //}

    static ComputeShader cshader;
    static int kernelHandle;

    static CleanImage()
    {
        cshader = (ComputeShader)Resources.Load("Shaders/CleanImageShader");
        kernelHandle = cshader.FindKernel("CSMain");
    }

    public static void CleanCompute(ref RenderTexture renderTexture)
    {
        cshader.SetTexture(kernelHandle, "Result", renderTexture);
        cshader.Dispatch(kernelHandle, renderTexture.width / 8, renderTexture.height / 2, 1);
    }
}
