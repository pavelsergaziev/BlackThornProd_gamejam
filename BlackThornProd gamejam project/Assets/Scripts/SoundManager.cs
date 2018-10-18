using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour {
    [SerializeField]
    private AudioSource _musicSource;
    [SerializeField]
    private AudioSource _fxSource;
    [SerializeField]
    private AudioClip [] _audioClips;

    private void Start()
    {
        StartNewGame();
    }

    public void PlaySound(string name, AudioSource source)
    {
        source.clip = Array.Find(_audioClips, audioClip => audioClip.name == name);
        source.Play();
    }
    public void PlaySound(string name, AudioSource source,bool loop)
    {
        source.clip = Array.Find(_audioClips, audioClip => audioClip.name == name);
        if (loop)
        {
            source.loop = true;
        }
        else
        {
            source.loop = false;
        }
        source.Play();
        
    }
    IEnumerator PlayAfter(string name, AudioSource source)
    {
        while (_musicSource.isPlaying)
        {
            yield return null;
        }
        PlaySound(name, source, true);
    }
    public void StartNewGame()
    {
        PlaySound("Chill+introMus", _musicSource);
        StartCoroutine(PlayAfter("MainThemeMus", _musicSource));
    }
    public void DeadMenu()
    {
        PlaySound("DeadMenuMus", _musicSource, true);
    }
}
