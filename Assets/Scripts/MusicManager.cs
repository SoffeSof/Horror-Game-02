using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; } // Singleton to ensure only one MusicManager exists

    [SerializeField] private AudioSource audioSource; // AudioSource component to play the music
    [SerializeField] private AudioClip normalMusic; // Normal background music
    [SerializeField] private AudioClip chasingMusic; // Music when being chased

    private void Awake()
    {
        // Singleton pattern to ensure there's only one MusicManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy any duplicate instances of MusicManager
        }
        else
        {
            Instance = this; // Set this instance as the singleton
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }

        // If no AudioSource is attached, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource if it's missing
        }
        audioSource.clip = normalMusic; // Set the initial music clip to normalMusic
    }

    private void Start()
    {
        // Ensure the audio source is set up for looping
        audioSource.loop = true; // Make the music loop
        audioSource.Play(); // Start playing the normal music
    }

    public void ChangeMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Stop the current music if it's playing
        }
        audioSource.clip = chasingMusic; // Change the music clip to chasingMusic
        audioSource.Play(); // Play the new chasing music
    }
}
