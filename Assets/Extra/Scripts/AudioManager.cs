using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst;
    public AudioSource Source;
    public Sound[] clips;

    public void Awake()
    {
        inst = this;
        Debug.Log("Pause");
        Source.Stop();
    }

    public void PlaySound(SoundName name)
    {
        AudioSource[] audioSources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i] != Source)
            {
                audioSources[i].Pause();
            }
        }

        foreach (var item in clips)
        {
            if (item.name == name)
            {
                Source.clip = item.clip;
                Source.Play();
                break;
            }
        }
    }

    public bool IsAudioPlaying()
    {
        return Source.isPlaying;
    }

    public void SoundMute(bool val)
    {
        Source.mute = val;   
    }

    public void SoundUnmute(bool val)
    {
        Source.mute = val;
    }
}

[System.Serializable]
public class Sound
{
    public SoundName name;
    public AudioClip clip;
}

public enum SoundName
{
    welcome,
    startEngine,
    throttleThrust,
    cyclicThrottle,
    takeoff,
}
