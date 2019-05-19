using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAI : MonoBehaviour
{
    [Header("Requests")]

    [Tooltip("How many request of each type in this level, id 0 is small, 1 medium, 2 big, 3 around")]
    public int[] requestCount = new int[4];

    List<MagicRequest> requests = new List<MagicRequest>();

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
                requests.Add(new MagicRequest((MagicRequest.Type)i));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ThrowRequests());
    }

    IEnumerator ThrowRequests()
    {
        while(requests.Count> 0)
        {
            yield return new WaitForSeconds(Random.Range(minRequestInterval, maxRequestInterval));

            //Throw a new request
            MagicRequest currentRequest = ExecuteNewRequest();

            //Wait for the anim to finish before doing anything else
            while (animIsPlaying)
                yield return null;

            //Tell the gamemanager which request has been thrown
            GameManager.Instance.AddNewRequest(currentRequest);
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
}

[System.Serializable]
public struct MagicRequest
{
    public enum Type { Small,Medium,Big,Around}
    public enum Direction { Right, Left,Both}
    public int pixelValue
    {
        get
        {
            switch (type)
            {
                case Type.Small:
                    return 5;
                case Type.Medium:
                    return 20;
                case Type.Big:
                    return 50;
                case Type.Around:
                    return 40;
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
