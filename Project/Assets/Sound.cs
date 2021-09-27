using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume;
    public bool shouldLoop;
    [HideInInspector] public AudioSource src;
} 
