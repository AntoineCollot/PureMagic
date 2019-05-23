using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Talk(bool value)
    {
        GetComponent<Animator>().SetBool("TalkingToPlayer", value);
    }
}
