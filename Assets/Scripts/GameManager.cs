using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<PixelRequest> leftRequests= new List<PixelRequest>();
    List<PixelRequest> rightRequests = new List<PixelRequest>();

    public float timePerRequest;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerMagic.Instance.onPlayerDraw.AddListener(OnPlayerDraw);
    }

    public void OnPlayerDraw(int pixelClearedCount)
    {
        //Check on which side of the screen we are
        bool isDrawingRight = Input.mousePosition.x > Screen.width * 0.5f;

        if(isDrawingRight)
        {
            ApplyPixelsToRequest(rightRequests, pixelClearedCount);
        }
        else
        {
            ApplyPixelsToRequest(leftRequests, pixelClearedCount);
        }
    }

    void LateUpdate()
    {
        if(LookForGameOver(leftRequests) || LookForGameOver(rightRequests))
        {
            //Gameover
            Debug.Log("GameOver");
        }
    }

    public bool LookForGameOver(List<PixelRequest> requests)
    {
        for (int i = 0; i < requests.Count; i++)
        {
            if (requests[i].IsFailed)
            {
                return true;
            }
        }
        return false;
    }

    public void ApplyPixelsToRequest(List<PixelRequest> requests, int pixelCount)
    {
        if(requests.Count>0)
        {
            requests[0].pixelCleared += pixelCount;

            //Check if the request is completed so we can remove it
            if (requests[0].IsCleared)
            {
                requests.RemoveAt(0);
            }
        }
    }

    public void AddNewRequest(MagicRequest magicRequest)
    {
        PixelRequest pixelRequest = new PixelRequest();
        pixelRequest.pixelCountRequested = magicRequest.pixelValue;
        pixelRequest.endTime = Time.time + timePerRequest;

        switch (magicRequest.direction)
        {
            case MagicRequest.Direction.Right:
                rightRequests.Add(pixelRequest);
                break;
            case MagicRequest.Direction.Left:
                leftRequests.Add(pixelRequest);
                break;
        }
    }

    public class PixelRequest
    {
        public int pixelCountRequested;
        public int pixelCleared;
        public float endTime;

        public bool IsCleared
        {
            get
            {
                return pixelCleared >= pixelCountRequested;
            }
        }

        public bool IsFailed
        {
            get
            {
                return !IsCleared && Time.time >= endTime;
            }
        }
    }
}
