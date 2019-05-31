using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAI : MonoBehaviour
{
    [Header("Requests")]

    [Tooltip("How many request of each type in this level")]
    public MagicRequestAmount[] requestCount;

    List<MagicRequest> requests = new List<MagicRequest>();
    int startRequestCount;
    public float Progress
    {
        get
        {
            return 1.0f - (float)requests.Count / startRequestCount;
        }
    }

    [Header("Parameters")]

    public float startDelay = 2;
    public float maxRequestInterval;
    public float minRequestInterval;

    [Header("Animations")]

    Animator anim;
    //is turned back to false diretly in the animator
    public static bool animIsPlaying = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        //Populate the request list
        for (int i = 0; i < requestCount.Length; i++)
        {
            for (int r = 0; r < requestCount[i].count; r++)
            {
                requests.Add(new MagicRequest(requestCount[i].type, requestCount[i].direction));
            }
        }

        startRequestCount = requests.Count;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ThrowRequests());

        GameManager.Instance.onLevelCleared.AddListener(OnLevelCleared);
        GameManager.Instance.onLevelFailed.AddListener(OnLevelFailed);
    }

    IEnumerator ThrowRequests()
    {
        yield return new WaitForSeconds(startDelay- maxRequestInterval);

        while (requests.Count> 0 && !GameManager.gameIsOver)
        {
            yield return new WaitForSeconds(Mathf.Lerp(minRequestInterval, maxRequestInterval, Progress));

            //Throw a new request
            MagicRequest currentRequest = ExecuteNewRequest();

            //Tell the gamemanager which request has been thrown
            RequestsManager.Instance.AddNewRequest(currentRequest);

            if(requests.Count==0)
            {
                //Tell the game manager that we finished asking for stuff
                RequestsManager.Instance.requestsAreOver = true;
            }

            //Wait for the anim to finish before doing anything else
            while (animIsPlaying)
                yield return null;
        }
    }

    public MagicRequest ExecuteNewRequest()
    {
        int rand = Random.Range(0, requests.Count);
        MagicRequest request = requests[rand];
        requests.RemoveAt(rand);


        if(request.direction != MagicRequest.Direction.Both)
            anim.SetBool("Right", request.IsRight);
        anim.SetInteger("SpellType", request.SpellId);
        anim.SetTrigger("Spell");
        animIsPlaying = true;

        return request;
    }

    void OnLevelCleared()
    {
        anim.SetBool("Right", false);
        anim.SetBool("Happy", true);
    }

    void OnLevelFailed()
    {
        anim.SetBool("Right", false);
        anim.SetBool("Sad", true);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        int pixelRequiered = 0;
        for (int i = 0; i < requestCount.Length; i++)
        {
            if(requestCount[i].type == MagicRequest.Type.Around)
                pixelRequiered += MagicRequest.GetPixelValue(requestCount[i].type) * requestCount[i].count * 2;
            else
                pixelRequiered += MagicRequest.GetPixelValue(requestCount[i].type) * requestCount[i].count;
        }

        Debug.Log("Pixel requiered : "+ pixelRequiered);
    }
#endif
}

[System.Serializable]
public struct MagicRequest
{
    public static int smallSpellPixels = 100;
    public static int mediumSpellPixels = 350;
    public static int bigSpellPixels = 700;
    public static int aroundSpellPixels = 350;

    public enum Type { Small,Medium,Big,Around}
    public enum Direction { Right, Left,Both}

    public int pixelValue
    {
        get
        {
            switch (type)
            {
                case Type.Small:
                    return smallSpellPixels;
                case Type.Medium:
                    return mediumSpellPixels;
                case Type.Big:
                    return bigSpellPixels;
                case Type.Around:
                    return aroundSpellPixels;
                default:
                    return 0;
            }
        }
    }

    public static int GetPixelValue(Type type)
    {
        switch (type)
        {
            case Type.Small:
                return smallSpellPixels;
            case Type.Medium:
                return mediumSpellPixels;
            case Type.Big:
                return bigSpellPixels;
            case Type.Around:
                return aroundSpellPixels;
            default:
                return 0;
        }
    }

    public MagicRequest(Type type,Direction direction)
    {
        this.type = type;
        this.direction = direction;
    }

    public int SpellId
    {
        get
        {
            return type.GetHashCode();
        }
    }

    public bool IsRight
    {
        get
        {
            return direction == Direction.Right;
        }
    }

    public Type type;
    public Direction direction;
}

[System.Serializable]
public struct MagicRequestAmount
{
    public MagicRequest.Type type;
    public MagicRequest.Direction direction;
    public int count;
}
