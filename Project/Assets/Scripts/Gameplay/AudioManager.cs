using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> sounds;
    private Player player;
    
    private bool isPaused;
    private bool isPlaying;

    private void Awake()
    {
        isPlaying = SceneManager.GetActiveScene().buildIndex == 1;
        
        if (isPlaying)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        foreach (Sound sound in sounds)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();

            src.clip = sound.clip;
            src.volume = sound.volume;
            src.loop = sound.shouldLoop;
            sound.src = src;
        }
    }

    private void Start()
    {
        if (isPlaying)
            PlayClip("backgroundMusic");
    }

    private void Update()
    {
        if (isPlaying && player.isDead)
        {
            PauseClip("backgroundMusic");
            isPaused = true;
        }
        else if (isPlaying && !player.isDead)
        {
            UnPauseClip("backgroundMusic");
            isPaused = false;
        }
    }

    public void PauseClip(string name)
    {
        Sound sound = sounds.Find(x => x.name == name);
        if (sound != null && sound.src.isPlaying)
        {
            sound.src.Pause();
        }
    }

    public void UnPauseClip(string name)
    {
        Sound sound = sounds.Find(x => x.name == name);
        if (sound != null)
        {
            sound.src.UnPause();
        }
    }

    public void StopClip(string name)
    {
        Sound sound = sounds.Find(x => x.name == name);
        if (sound != null && sound.src.isPlaying)
        {
            sound.src.Stop();
        }
    }

    public void PlayClipRandomized(string name)
    {
        Sound toPlay = sounds.Find(x => x.name == name);
        toPlay.src.pitch = Random.Range(1, 1.01f);
        toPlay.src.Play();
    }

    public void PlayClip(string name)
    {
        Sound toPlay = sounds.Find(x => x.name == name);
        if (toPlay != null)
            toPlay.src.Play();
    }
}
