using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Door door; // Reference to the door that this key unlocks.
    public Interactable doorInteractable; // Reference to the interactable component on the door.
    public Interactable interactable; // Reference to the interactable component on this key.
    public KeyInventoryManager keyInventoryManager; // Reference to the manager that handles key inventory.

    public string keyName; // Name of the key, used to identify it.
    public int keyNumber; // Number associated with the key.

    public void Awake()
    {
        interactable = GetComponent<Interactable>(); // Get the interactable component attached to this key.
        keyInventoryManager = FindObjectOfType<KeyInventoryManager>(); // Find and assign the KeyInventoryManager in the scene.

        interactable.displayMessage = "Pick up Key"; // Set the message displayed to the player when interacting with this key.

         if (door != null) // Check if the key has a door linked to it.
        {
            doorInteractable = door.interactable; // Get the interactable component from the linked door.
        }
        else
        {
            Debug.LogError("Key does not have a door linked to it"); // Log an error if no door is linked.
        }
    }
    
    public void AddToInventory() // Adds this key to the inventory system
    {
        keyInventoryManager.AddNoteToInventory(keyName, keyNumber); // Add the key to the inventory.
        door.hasKey = true; // Set the door's "hasKey" property to true, allowing it to be unlocked.
        door.SetUnlockDisplayMessage(); // Update the door's display message to indicate it can be unlocked.
        Destroy(gameObject); // Destroy the key object in the scene after it's picked up.
    }
}
