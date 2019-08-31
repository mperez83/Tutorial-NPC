using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public Sound[] sounds;
    AudioSource audioSource;

    private void Awake() 
    {
        if (instance != null)
            Destroy(gameObject); 
        else 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Play("Menu Scene Soundtrack");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " was not found");
            return;
        }

        if (audioSource.isPlaying) audioSource.Stop();

        audioSource.clip = s.clip;
        audioSource.outputAudioMixerGroup = s.output;

        audioSource.volume = s.volume;
        audioSource.pitch = s.pitch;
        audioSource.loop = s.loop;

        audioSource.Play();
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " was not found");
            return;
        }

        if (audioSource.isPlaying) audioSource.Stop();
    }

}
