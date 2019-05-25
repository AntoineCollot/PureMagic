using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAI : MonoBehaviour
{
    [Header("Requests")]

    [Tooltip("How many request of each type in this level, id 0 is small, 1 medium, 2 big, 3 around")]
    public int[] requestCount = new int[4];

    List<MagicRequest> requests = new List<MagicRequest>();
    int startRequestCount;
    public float Progress
    {
        get
        {
            return (float)requests.Count / startRequestCount;
        }
    }

    [Header("Parameters")]

    public float minRequestInterval;
    public float maxRequestInterval;

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
            for (int r = 0; r < requestCount[i]; r++)
            {
                if(requestCount[i]==1)
                    requests.Add(new MagicRequest((MagicRequest.Type)i,false));
                else
                    requests.Add(new MagicRequest((MagicRequest.Type)i));
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
        while(requests.Count> 0 && !GameManager.gameIsOver)
        {
            yield return new WaitForSeconds(Mathf.Lerp(maxRequestInterval, minRequestInterval, Progress));

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
        if (requestCount.Length < 4)
            return;

        int pixelRequiered = 0;
        pixelRequiered += requestCount[0] * MagicRequest.smallSpellPixels;
        pixelRequiered += requestCount[1] * MagicRequest.mediumSpellPixels;
        pixelRequiered += requestCount[2] * MagicRequest.bigSpellPixels;
        pixelRequiered += requestCount[3] * MagicRequest.aroundSpellPixels *2;

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

    public MagicRequest(Type type)
    {
        this.type = type;
        direction = Direction.Both;
        if(type!=Type.Around)
            direction = GetPseudoRandomDirection();
    }

    public MagicRequest(Type type, bool right)
    {
        this.type = type;
        direction = Direction.Both;
        if (type != Type.Around && right)
            direction = Direction.Right;
        else if(type != Type.Around)
            direction = Direction.Left;
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

    public static Queue<Direction> availableDirections = new Queue<Direction>();
    public Direction GetPseudoRandomDirection()
    {
        if(availableDirections.Count==0)
        {
 
            if(Random.Range(0.0f,1.0f)>0.5f)
            {
                availableDirections.Enqueue(Direction.Right);
                availableDirections.Enqueue(Direction.Left);
            }
            else
            {
                availableDirections.Enqueue(Direction.Left);
                availableDirections.Enqueue(Direction.Right);
            }
        }

        return availableDirections.Dequeue();
    }

    public Type type;
    public Direction direction;
}
