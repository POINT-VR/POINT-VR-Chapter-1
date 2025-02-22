using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialAudioManager : MonoBehaviour
{
    public static TutorialAudioManager Instance;

    public TutorialAudio[] musicAudio, sfxAudio, voiceAudio;
    public AudioSource musicSource, sfxSource, voiceSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Tutorial I saw showed how to initialize the theme music
    //private void Start()
    //{
    //    PlayMusic("MainTheme");
    //}
   
    
    
    public void PlayMusic(string name)
    {
        TutorialAudio audio = Array.Find(musicAudio, sound => sound.name == name);
        if (audio == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        else
        {
            musicSource.clip = audio.clip;
            musicSource.Play();
        }
        
    }           
    
    public void PlaySFX(string name)
    {
        TutorialAudio audio = Array.Find(sfxAudio, sound => sound.name == name);
        if (audio == null)
        {
            Debug.LogWarning("SFX: " + name + " not found!");
            return;
        }
        else
        {
            sfxSource.clip = audio.clip;
            sfxSource.Play();  
        }
        
    }
    
    public void PlayVoice(string name)
    {
        TutorialAudio audio = Array.Find(voiceAudio, sound => sound.name == name);
        if (audio == null)
        {
            Debug.LogWarning("Voice: " + name + " not found!");
            return;
        }
        else
        {
            voiceSource.clip = audio.clip;
            voiceSource.Play();

        }
        
    }
}

