
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum Move { SpellSmall,SpellMedium,SpellBig,SpellAround,Happy,Sad,Hey}

    public GirlSFX[] girlSFX;

    new AudioSource audio;

    public static AudioManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        audio = GetComponent<AudioSource>();
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
