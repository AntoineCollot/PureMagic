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
        //If the player cleared any pixel
        if(pixels>0)
        {
            //If they is a request ongoing, it's ok to draw
            if(RequestsManager.Instance.requestsCount>0)
            {
                return;
            }

            //If the game is over, it's too late to react
            if (GameManager.gameIsOver)
                return;

            gameObject.SetActive(false);
            activateOnInterrupt.SetActive(true);
        }
    }
}
