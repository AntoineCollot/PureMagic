using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestValidationEffect : MonoBehaviour
{
    ParticleSystem particles;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.onRequestCompleted.AddListener(OnRequestCompleted);   
    }

    public void OnRequestCompleted()
    {
        particles.Play();
    }
}
