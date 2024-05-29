using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSoundTrack : MonoBehaviour
{
    public static UpdateSoundTrack Instance;
    private AudioSource[] audioSources;
    private void Start()
    {
        Instance = this;
        audioSources = GetComponents<AudioSource>();
        UpdateSound();
    }

    public void UpdateSound() 
    {
        foreach (AudioSource source in audioSources)
        {
            source.volume = PlayerPrefs.GetFloat("SoundVolume")/100;
        }
    }



    public void MuteByIndex(int index) 
    {
        audioSources[index].mute = true;
    }

    public void UnmuteByIndex(int index) 
    {
        audioSources[index].mute = false;
    }

    public void MuteAll() 
    {
        foreach (AudioSource source in audioSources)
        {
            source.mute = true;
        }
    }

    public void UnmuteAll() 
    {
        foreach (AudioSource source in audioSources)
        {
            source.mute = false;
        }
    }
}
