using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterfliesEmission : MonoBehaviour
{
    ParticleSystem particles;

    public int larvaPixelCleared;
    public int larvaPixelPerButterfly;

    // Start is called before the first frame update
    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        PlayerMagic.Instance.onPlayerDraw.AddListener(OnPlayerDraw);
    }

    void OnPlayerDraw(int pixelCleared)
    {
        larvaPixelCleared += pixelCleared;

        //If enough pixels are cleared to spawn a butterfly
        if(larvaPixelCleared/larvaPixelPerButterfly>0)
        {
            particles.Emit(larvaPixelCleared / larvaPixelPerButterfly);
            larvaPixelCleared %= larvaPixelPerButterfly;
        }
    }
}
