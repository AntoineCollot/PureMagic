
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Move { SpellSmall,SpellMedium,SpellBig,SpellAround,Happy,Sad,Hey}

    public GirlSFX[] girlSFX;

    public AudioClip[] successClips;

    new AudioSource audio;
    public AudioSource butterflyAudio;
    public float butterlyVolume = 0.5f;
    float targetButterlyVolume = 0f;
    float refButterlyVolume;

    public static AudioManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        audio = GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if(DrawOnTexture.Instance.deltaPixelDrawn>0 && Input.GetMouseButton(0))
        {
            targetButterlyVolume = butterlyVolume;
        }
        else
        {
            targetButterlyVolume = 0;
        }

        butterflyAudio.volume = Mathf.SmoothDamp(butterflyAudio.volume, targetButterlyVolume, ref refButterlyVolume, 0.2f);
    }

    public void PlayGirlSFX(Move move)
    {
        foreach(GirlSFX sfx in girlSFX)
        {
            if(sfx.move == move)
            {
                PlayClip(sfx.RandomClip, sfx.volume);
            }
        }
    }

    public void PlayClip(AudioClip clip, float volume =1)
    {
        audio.PlayOneShot(clip, volume);
    }

    public void PlaySuccess()
    {
        PlayClip(successClips[Random.Range(0, successClips.Length)],0.5f);
    }

    [System.Serializable]
    public struct GirlSFX
    {
        public Move move;
        public AudioClip[] clips;
        public float volume;

        public AudioClip RandomClip
        {
            get
            {
                return clips[Random.Range(0, clips.Length)];
            }
        }
    }
}
