using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBase : MonoBehaviour
{
    public bool hasLever = false; // Indicates whether the lever has been inserted.
    public bool pullLever = false; // Indicates if the lever is ready to be pulled.
    private bool hasBeenOpened = false; // Ensures the gate only opens once after the lever is pulled.
    private string leverMissingText = "The lever is missing"; // Text displayed when the lever is not inserted.
    private string insertleverText = "Insert lever"; // Text displayed when the lever is ready to be inserted.
    private string pullLeverText = "Pull lever"; // Text displayed when the lever is ready to be pulled.
    private Interactable interactable; // The Interactable component that allows user interaction.
    public InventoryManager inventoryManager; // Reference to the inventory manager to manage items.
    public GameObject lever; // The lever GameObject to be activated.
    public GameObject gate; // The gate GameObject that will open when the lever is pulled.
    public Item leverItem; // Reference to the lever item in the inventory.
    public AudioSource openSound; // Sound that plays when the gate is opened.


    private void Awake()
    {
        interactable = GetComponent<Interactable>(); // Get the Interactable component attached to this GameObject.
        inventoryManager = FindObjectOfType<InventoryManager>(); // Find the InventoryManager in the scene.
        interactable.displayMessage = leverMissingText; // Set the initial message when the lever is missing.
    }

    public void InsertLever()
    {
        if (hasBeenOpened)  //If the gate has already been opened, prevent further actions.
        {
            return; // Exit the method
        }
        if (pullLever) // If the lever is ready to be pulled.
        {
            PullLeverOpenGate(); // Trigger the gate opening sequence.
            hasBeenOpened = true; // Ensure the gate is only opened once.
        }
        if (hasLever) // If the lever has been inserted into the leverbase.
        {
            lever.SetActive(true); // Activate the lever object to appear in the LeverBase GO
            inventoryManager.RemoveItem(leverItem); // Remove the lever item from the inventory.
            interactable.displayMessage = pullLeverText; // Update the message to prompt the player to pull the lever.
            pullLever = true; // Set the pullLever bool to true, indicating the lever can be pulled.
        }
    }

    public void AddLever()
    {
        hasLever = true; // Set the hasLever flag to true when a lever is added.
        interactable.displayMessage = insertleverText; // Update the message to prompt the player to insert the lever.
    }

    public void PullLeverOpenGate()
    {
        StartCoroutine(RotateLeverRaiseGate()); // Start the coroutine to rotate the lever and open the gate.
    }
    
    private IEnumerator RotateLeverRaiseGate()
    {
        float duration = 5f; // Duration of the rotation and door opening animation (in seconds).
        float elapsedTime = 0f; // Timer to track the elapsed time during the animation.

        // Define the starting and target rotations for the lever.
        Quaternion startRotation = lever.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(45, 0, 0); // The final rotation of the lever (rotating 45 degrees).

        // Define the starting and target positions for the door.
        Vector3 startDoorPosition = gate.transform.position; // The initial position of the door as current position
        Vector3 endDoorPosition = startDoorPosition + new Vector3(0, 5, 0); // Move the door upward by 5 units.
        openSound.Play();// Play the sound effect for opening the door.

        while (elapsedTime < duration) // While the elapsed time is less than the duration of the animation.
        {
            elapsedTime += Time.deltaTime; // Increment the elapsed time by the time between frames.

            // Rotate the lever
            lever.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration); // Rotate the lever smoothly from start to end rotation using spherical linear interpolation (Slerp).
            // Move the gate
            gate.transform.position = Vector3.Lerp(startDoorPosition, endDoorPosition, elapsedTime / duration); // Move the door smoothly from the start to end position using linear interpolation (Lerp).
            yield return null; // Wait for the next frame.
        }

        // Ensure the lever ends exactly at the target rotation
        lever.transform.rotation = endRotation;
        gate.transform.position = endDoorPosition;
    }
}
