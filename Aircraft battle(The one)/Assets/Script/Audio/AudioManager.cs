using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] public AudioSource SFX_Player;
    [SerializeField] public AudioSource BGM_Player;
    [SerializeField] public AudioSource AmBient_Player;
    public void playSFX(AudioData audiodata)
    {    
        
        SFX_Player.PlayOneShot(audiodata.audioClip,audiodata.volume);
    }

    public void PlayRandomSFX(AudioData audiodata)
    {
        SFX_Player.pitch=Random.Range(0.9f, 1.1f);
        SFX_Player.PlayOneShot(audiodata.audioClip, audiodata.volume);
    }
    public void PlayRandomSFX(AudioData[] audiodata)
    {
        PlayRandomSFX(audiodata[Random.Range(0,audiodata.Length)]);
    }
}
