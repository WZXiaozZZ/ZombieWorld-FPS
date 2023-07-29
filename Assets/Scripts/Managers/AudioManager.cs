using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicName
{
    Walk,
    Run,
    Rifle_01,
}
[System.Serializable]
public struct MusicType
{
    public AudioClip musicClip;
    public MusicName musicName;
    [Range(0f, 1f)] public float volume;
}


public class AudioManager : MonoBehaviour
{
    private AudioSource BGMSource;
    private AudioSource FXSource;
    private AudioSource PlayerSource;
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public MusicType[] musicTypes;
    private Dictionary<MusicName, MusicType> musicDictionary = new Dictionary<MusicName, MusicType>();
    public void Awake()
    {
        _instance = this;
        foreach (var item in musicTypes)
        {
            musicDictionary.Add(item.musicName, item);
        }
        PlayerSource = gameObject.AddComponent<AudioSource>();
        BGMSource = gameObject.AddComponent<AudioSource>();
        FXSource = gameObject.AddComponent<AudioSource>();
    }



    public void PlayPlayerMusic(AudioClip clip, float volume)
    {
        PlayerSource.clip = clip;
        if (!PlayerSource.isPlaying)
        {
            PlayerSource.volume = volume;
            PlayerSource.Play();
        }
    }
    public void PlayPlayerMusic(MusicName name)
    {
        PlayerSource.clip = musicDictionary[name].musicClip;
        if (!PlayerSource.isPlaying)
        {
            PlayerSource.volume = musicDictionary[name].volume;
            PlayerSource.Play();
        }
    }
    public void PlayMusicFX(MusicName name)
    {
        FXSource.clip = musicDictionary[name].musicClip;
        if (!FXSource.isPlaying)
        {
            FXSource.volume = musicDictionary[name].volume;
            FXSource.Play();
        }
    }

    public void PlayOneShotMusicFX(MusicName name)
    {
        FXSource.volume = musicDictionary[name].volume;
        FXSource.PlayOneShot(musicDictionary[name].musicClip);
    }

    public void StopPlayerMusic() 
    {
        PlayerSource.Stop();
    }
   
}
