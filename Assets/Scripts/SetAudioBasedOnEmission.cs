using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAudioBasedOnEmission : MonoBehaviour
{
   new  AudioSource audio;
    public ParticleSystem particles;

    public int maxParticles = 100;

    private void Start()
    {
        audio=GetComponent<AudioSource>();
    }

    private void Update()
    {
        audio.volume = Mathf.Lerp(0,1,(float)particles.particleCount/maxParticles);
    }
}
