using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RequestsManager : MonoBehaviour
{
    public static RequestsManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    List<PixelRequest> leftRequests = new List<PixelRequest>();
    List<PixelRequest> rightRequests = new List<PixelRequest>();

    [System.Serializable]
    public class RequestEvent : UnityEvent<PixelRequest> { }
    public RequestEvent onNewRequest = new RequestEvent();
    public RequestEvent onRequestCompleted = new RequestEvent();

    public float timePerRequest = 2;

    [HideInInspector]
    public bool requestsAreOver = false;

    public int requestsCount
    {
        get
        {
            return leftRequests.Count + rightRequests.Count;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerMagic.Instance.onPlayerDraw.AddListener(OnPlayerDraw);
    }

    public void OnPlayerDraw(int pixelClearedCount)
    {
        if (GameManager.gameIsOver)
            return;

        //Check on which side of the screen we are
        bool isDrawingRight = Input.mousePosition.x > Screen.width * 0.5f;

        if (isDrawingRight)
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
        if (LookForGameOver(leftRequests) || LookForGameOver(rightRequests))
        {
            //Gameover
            GameManager.Instance.GameOver();
        }
    }

    public bool LookForGameOver(List<PixelRequest> requests)
    {
        for (int i = 0; i < requests.Count; i++)
        {
            if (requests[i].IsFailed)
            {
                requests.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public void ApplyPixelsToRequest(List<PixelRequest> requests, int pixelCount)
    {
        if (requests.Count > 0)
        {
            requests[0].pixelCleared += pixelCount;

            //Check if the request is completed so we can remove it
            if (requests[0].IsCleared)
            {
                onRequestCompleted.Invoke(requests[0]);

                requests.RemoveAt(0);

                AudioManager.Instance.PlaySuccess();

                //Check if the level is cleared
                if (CheckGameEnded())
                {
                    GameManager.Instance.ClearLevel();
                }
            }
        }
    }

    public void AddNewRequest(MagicRequest magicRequest)
    {
        PixelRequest pixelRequest = new PixelRequest();
        pixelRequest.name = magicRequest.type.ToString();
        pixelRequest.pixelCountRequested = magicRequest.pixelValue;
        pixelRequest.endTime = Time.time + timePerRequest;

        switch (magicRequest.direction)
        {
            case MagicRequest.Direction.Right:
                rightRequests.Add(pixelRequest);
                onNewRequest.Invoke(pixelRequest);
                break;
            case MagicRequest.Direction.Left:
                leftRequests.Add(pixelRequest);
                onNewRequest.Invoke(pixelRequest);
                break;
            case MagicRequest.Direction.Both:
                //Add a little bit of time if its around
                pixelRequest.endTime = Time.time + timePerRequest * 1.5f;

                leftRequests.Add(pixelRequest);
                PixelRequest pixelRequest2 = new PixelRequest();
                pixelRequest2.name = pixelRequest.name;
                pixelRequest2.pixelCountRequested = pixelRequest.pixelCountRequested;
                pixelRequest2.endTime = pixelRequest.endTime;
                rightRequests.Add(pixelRequest2);

                onNewRequest.Invoke(pixelRequest);
                onNewRequest.Invoke(pixelRequest2);

                break;
        }
    }

    public float GetRequestProgress(bool right)
    {
        if (right)
        {
            if (rightRequests.Count > 0)
            {
                return (float)rightRequests[0].pixelCleared / rightRequests[0].pixelCountRequested;
            }
            else
                return 1;
        }
        else
        {
            if (leftRequests.Count > 0)
            {
                return (float)leftRequests[0].pixelCleared / leftRequests[0].pixelCountRequested;
            }
            else
                return 1;
        }
    }

    public bool CheckGameEnded()
    {
        return leftRequests.Count == 0 && rightRequests.Count == 0 && requestsAreOver;
    }

    public class PixelRequest
    {
        public string name;
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
#if UNITY_EDITOR
                if (!IsCleared && Time.time >= endTime)
                    Debug.Log("Failed " + name + "(" + pixelCleared + "/" + pixelCountRequested + ")");
#endif
                return !IsCleared && Time.time >= endTime;
            }
        }

        public override string ToString()
        {
            return name + "(" + pixelCleared + "/" + pixelCountRequested + ")";
        }
    }

    //#if UNITY_EDITOR
    //    private void OnGUI()
    //    {
    //        Rect rect = new Rect(10, 10, 300, 100);
    //        GUI.Label(rect, "Right");
    //        rect.y += 30;
    //        foreach (PixelRequest request in rightRequests)
    //        {
    //            GUI.Label(rect, request.ToString());
    //            rect.y += 30;
    //        }
    //        GUI.Label(rect, "Left");
    //        rect.y += 30;
    //        foreach (PixelRequest request in leftRequests)
    //        {
    //            GUI.Label(rect, request.ToString());
    //            rect.y += 30;
    //        }



    //    }
    //#endif
}
