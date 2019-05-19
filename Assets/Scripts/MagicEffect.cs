using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEffect : MonoBehaviour
{
    ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            particles.Play();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            particles.Stop();
        }
    }
}
