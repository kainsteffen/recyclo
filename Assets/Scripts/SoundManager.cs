using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource[] sources;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        sources = GetComponents<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Play(string name)
    {
        AudioSource source = Array.Find(sources, sound => sound.clip.name == name);
        if(source)
        {
            source.Play();
        } else
        {
            print("Sound not found");
        }
    }

    public void Play(string name, float volume, float pitch)
    {
        AudioSource source = Array.Find(sources, sound => sound.clip.name == name);
        if (source)
        {
            source.volume = volume;
            source.pitch = pitch;
            source.Play();
        }
        else
        {
            print("Sound not found");
        }
    }

    public void PlayLoop(string name)
    {
        AudioSource source = Array.Find(sources, sound => sound.clip.name == name);
        if (source)
        {
            source.loop = true;
            source.Play();
        }
        else
        {
            print("Sound not found");
        }
    }

    public void Stop(string name)
    {
        AudioSource source = Array.Find(sources, sound => sound.clip.name == name);
        if (source)
        {
            source.Stop();
        }
        else
        {
            print("Sound not found");
        }
    }

    public void FadeIn(string name)
    {
        AudioSource source = Array.Find(sources, sound => sound.clip.name == name);
        source.DOFade(1, 0.7f);
    }

    public void FadeOut(string name)
    {
        AudioSource source = Array.Find(sources, sound => sound.clip.name == name);
        source.DOComplete();
        source.DOFade(0, 0.7f);
    }

    public AudioSource Get(String name)
    {
        return Array.Find(sources, sound => sound.clip.name == name); 
    }
}
