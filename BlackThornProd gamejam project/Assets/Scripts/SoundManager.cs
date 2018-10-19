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
        //StartNewGame();
        MainMenu();
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
    public void StartNewGameWhithCutScene()
    {
        
        PlaySound("Chill+introMus", _musicSource,false);
        StartCoroutine(PlayAfter("MainThemeMus", _musicSource));
        StartCoroutine(PlayAfterSeconds(36, "DeadLineFx", true, _fxSource));
    }
    public void StartNewGameWhithoutCutScene()
    {
        PlaySound("MainThemeMus", _musicSource, true);
        StartCoroutine(PlayAfterSeconds(4, "DeadLineFx", true, _fxSource));

    }
    public void DeadMenu()
    {
        StopAllCoroutines();
        PlaySound("DeadMenuMus", _musicSource, true);
        
    }
    public void MainMenu()
    {
        _musicSource.pitch = 1f;
        PlaySound("MainMenuEmbience", _musicSource, true);
    }
    public void ButtonEnter()
    {
        PlaySound("ButtonEnterFx", _fxSource ,false);
    }
    public void ButtonClick()
    {
        PlaySound("ButtonClickFx", _fxSource, false);
    }
    public void UpdateWindows()
    {
        _musicSource.Stop();
        _fxSource.Stop();
        
        PlaySound("ErrorFx", _fxSource, false);
    }
    IEnumerator PlayAfterSeconds(float delay, string name, bool loop, AudioSource source)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(name, source, loop);
    }
    public void PauseMusic()
    {
        _musicSource.pitch = .5f;
    }
    public void ResumeMusic()
    {
        _musicSource.pitch = 1f;
    }
}
