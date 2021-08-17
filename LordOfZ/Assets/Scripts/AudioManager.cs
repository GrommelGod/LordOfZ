using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] sounds;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (Sound s in sounds)
            {
                AudioSource addedAudio = gameObject.AddComponent<AudioSource>();
                s.source = addedAudio;

                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.isLooping;
            }
        }
        else
        {
            DestroyImmediate(gameObject);
        }

    }

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Play("MainMusic");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            s.source.Stop();
        }
    }
}