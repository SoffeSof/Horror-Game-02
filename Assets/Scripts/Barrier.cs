using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Barrier : MonoBehaviour
{
    public bool hasMagicBall = false; // A boolean that tracks whether the player has collected the magic ball.
    public UnityEvent interactEvent; // // UnityEvent to handle interactions with the player. trigger specific actions when the player interacts with the barrier.

    
    public void AddMagicBall() // Method called when the player collects the magic ball. It sets the hasMagicBall flag to true and checks if the player can proceed.
    {
        hasMagicBall = true; // Set the magic ball flag to true.
        CheckForMagicBallAndNote(); // Check if the player has both the magic ball and all notes.
    }

    // Method to check if the player has both the magic ball and all notes collected.
    // If both conditions are true, it triggers the actions to run text, change music, and destroy the barrier.
    public void CheckForMagicBallAndNote()
    {
        if (hasMagicBall && HUDController.Instance.allNotesCollected == true) // Check if the player has the magic ball and if all notes have been collected.
        {
            HUDController.Instance.RunText();  // Calls the RunText method from the HUDController, that displays a message to the player.
            MusicManager.Instance.ChangeMusic(); // Change the background music.
            Destroy(gameObject); // Destroy the barrier object to allow progression in the game.
        }
    }

     // Method called when the player enters the trigger collider of the barrier.
    // Calls the "CantLeaveText" method from the HUDController to display a message to the player.
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactEvent.Invoke(); //Calls the event set in the inspector
        }
    }
}
