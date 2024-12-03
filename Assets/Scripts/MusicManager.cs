using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip chasingMusic;


    private void Awake()
    {
        // Singleton pattern to ensure there's only one MusicManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }

        // If no AudioSource is attached, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = normalMusic;
    }

    private void Start()
    {
        // Ensure the audio source is set up for looping
        audioSource.loop = true;
        audioSource.Play();
    }

    public void ChangeMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.clip = chasingMusic;
        audioSource.Play();
    }
}
