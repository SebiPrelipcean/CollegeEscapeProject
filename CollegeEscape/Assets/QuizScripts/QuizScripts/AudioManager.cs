using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable()]
public struct SoundParameters
{
    [Range(0, 1)]
    public float Volume;
    [Range(-3, 3)]
    public float Pitch;
    public bool Loop;
}
[System.Serializable()]
public class Sound
{

    [SerializeField]String name = String.Empty;
    public String GetName{ get { return name; } }

    [SerializeField]AudioClip audioClip = null;
    public AudioClip GetAudioClip{ get { return audioClip; } }

    [SerializeField]SoundParameters soundParameters = new SoundParameters();
    public SoundParameters GetSoundParameters{ get { return soundParameters; } }

    [HideInInspector]
    public AudioSource audioSource = null;

    public void Play ()
    {
        audioSource.clip = GetAudioClip;

        audioSource.volume = GetSoundParameters.Volume;
        audioSource.pitch = GetSoundParameters.Pitch;
        audioSource.loop = GetSoundParameters.Loop;

        audioSource.Play();
    }
    public void Stop ()
    {
        audioSource.Stop();
    }
}
public class AudioManager : MonoBehaviour
{
    public static   AudioManager audioInstance;

    [SerializeField] Sound[] sounds;
    [SerializeField] AudioSource sourcePrefab;
    [SerializeField] string startupTrack;

    void Awake(){
        if(audioInstance != null){
            Destroy(gameObject);
        }else{
            audioInstance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitSounds();
    }

    void Start(){
        if(string.IsNullOrEmpty(startupTrack) != true){
            PlaySound(startupTrack);
        }
    }

    void InitSounds(){
        foreach(var sound in sounds){
            AudioSource source = (AudioSource)Instantiate(sourcePrefab, gameObject.transform);
            source.name = sound.GetName;

            sound.audioSource = source;
        }
    }

    public void PlaySound(string pname){
        var sound = GetSound(pname);
        if(sound != null){
            sound.Play();
        }else{
            Debug.LogWarningFormat("Sound by the name {0} is not found for play", pname);
        }     
    }

    public void StopSound(string pname){
        var sound = GetSound(pname);
        if(sound != null){
            sound.Stop();
        }else{
            Debug.LogWarningFormat("Sound by the name {0} is not found for stop", pname);
        } 
    }

    Sound GetSound(string sname){
        foreach(var sound in sounds){
            if(sound.GetName == sname){
                return sound;
            }
        }
        return null;
    }
}
