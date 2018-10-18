using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Audio;
using UnityEngine;


class SoundOnObject : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _audioClips;
    private void Awake()
    {
        InitSources();
    }
    
    public void PlaySound(string name, bool loop)
    {
        AudioSource audioSource = Array.Find(GetComponents<AudioSource>(), source => source.clip.name == name);
        audioSource.loop = loop;
        audioSource.Play();
    }
    public void PlaySound(string name, bool loop, float volume)
    {
        AudioSource audioSource = Array.Find(GetComponents<AudioSource>(), source => source.clip.name == name);
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
    }
    private void InitSources()
    {
        foreach (var clip in _audioClips)
        {
            var audiosource = gameObject.AddComponent<AudioSource>();
            audiosource.playOnAwake = false;
            audiosource.loop = false;
            audiosource.clip = clip;

        }
    }
    public float GetTimeToEndOfClip(string name)
    {
        AudioSource audioSource = Array.Find(GetComponents<AudioSource>(), source => source.clip.name == name);
        return audioSource.clip.length - audioSource.time;
    }
    
}

