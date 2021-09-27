using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> sounds;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            
            src.clip = sound.clip;
            src.volume = sound.volume;
            src.loop = sound.shouldLoop;
            sound.src = src;
        }
    }

    public void PlayClip(string name)
    {
        Sound toPlay = sounds.Find(x => x.name == name);
        toPlay.src.Play();
    }
}
