using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptTutoOnDraw : MonoBehaviour
{
    public GameObject activateOnInterrupt;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMagic.Instance.onPlayerDraw.AddListener(OnPlayerDraw);
    }

    public void OnPlayerDraw(int pixels)
    {
        if(pixels>0)
        {
            gameObject.SetActive(false);
            activateOnInterrupt.SetActive(true);
        }
    }
}
