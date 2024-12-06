using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyInventoryManager : MonoBehaviour
{
    public TMP_Text[] keys; // Array of TextMeshProUGUI components to display the key names.
    public Button[] keyButtons; // Array of Buttons corresponding to each key for selection.
    public TMP_Text keyContent; // Text field to display the content of a selected key.
    public Transform keysParent; // Parent transform that contains the key display objects.

    public int keyCount = 0; // Counter to track how many keys have been collected.

    //Colors
    public Color unlockedColor; // Color to indicate a key has been unlocked.

    private string defaultText = "No key selected"; // Default text for the key content display
    private int currentlySelectedKey = -1; // Tracks the currently selected key (initialized to -1 for no selection).

    private Dictionary<int, string> keyContents = new Dictionary<int, string>() // Stores key content indexed by key number. (0-indexed)
    {
        { 0, "The hallway is where it all begins, the path you must walk again. The house knows you, Daniel, as it knows all those who dare to enter. These walls remember your footsteps, your promises, and your lies. Find your way through the darkness, but don't expect the house to let you leave." },
        { 1, "Once a place of warmth, now cold and forgotten. This key unlocks a space where hunger and desperation grew. The echoes of something more than meals linger here, something that hasn’t quite left. The house feeds on more than food, Daniel. And it is feeding on you." },
        { 2, "This key once belonged to those who tried to leave, only to become part of the house’s twisted history. Their cries are gone, but their presence remains. You, too, will leave something behind, Daniel, whether you choose to or not." },
        { 3, "The children’s laughter has long since stopped, but their whispers remain. The house holds its secrets in the spaces where the innocent once played. There is more here than just memories—there is something waiting. It is waiting for you." },
        { 4, "The attic is where the house hides what it wants to forget. But it cannot forget. Neither can you. The things stored here are not just memories, but pieces of a past that should never have been touched. This key opens the door to what you cannot unsee." },
        { 5, "This is the key to your escape, Daniel—or is it? The front door is the last barrier between you and what lies beyond the house. But can you truly leave? The house will never let you go, not completely. You’ve tried to run before, but it has a way of bringing you back. This key won’t open a door to freedom—it only unlocks another part of your past." }
    };

    void Start()
    {
        keys = keysParent.GetComponentsInChildren<TMP_Text>(); // Find all TextMeshProUGUI components under the parent.
        keyButtons = keysParent.GetComponentsInChildren<Button>(); // Find all Button components under the parent.
        
        // Set all note texts to "???"
        foreach (TMP_Text key in keys) // For each key text component in the array
        {
            if (key != null) // Ensure the note is not null to avoid errors
            {
                key.text = "???"; // Set the default text for locked keys.
            }
        }

        foreach (Button button in keyButtons)  // For each button in the array
        {
            if (button != null) // Ensure the note is not null to avoid errors
            {
                button.interactable = false;  // Buttons are not interactable until keys are unlocked.
            }
        }
    }

    // Method to add a key based on its number
    public void AddNoteToInventory(string keyName, int keyNumber) // Adds a key to the inventory.
    {
        if (keyNumber >= 0 && keyNumber < keys.Length) // Ensure the key number is within valid bounds.
        {
            keys[keyNumber].text = keyName; // Update the key display with its name.
            keys[keyNumber].color = unlockedColor; // Change the text color to indicate the key is unlocked.
            keyCount++; // Increment the total count of collected keys.

            if (keyNumber >= 0 && keyNumber < keyButtons.Length) // Enable the button for the corresponding key.
            {
                keyButtons[keyNumber].interactable = true; // Allow interaction with this key's button.
            }
        }
        else
        {
            Debug.LogWarning("Invalid key number"); // Log a warning if the key number is invalid.
        }
    }

    public void SelectKey(int keyNumber) // Selects a key to view its content.
    {
        if (currentlySelectedKey == keyNumber) // If the same key is selected again, deselect it.
        {
            DeselectKey(); // Deselect the key and return.
            return;
        }

        if (currentlySelectedKey != -1 && currentlySelectedKey < keys.Length) //If a different note is currently selected
        {
            keys[currentlySelectedKey].fontStyle = FontStyles.Normal;// Restore normal font style.
        } 
        
        currentlySelectedKey = keyNumber; // Update the currently selected key.

        if (keyContents.ContainsKey(currentlySelectedKey)) // Check if the selected key has content.
        {
            keyContent.text = keyContents[currentlySelectedKey]; // Display the key's content.
            keys[currentlySelectedKey].fontStyle = FontStyles.Bold; // Highlight the selected key text.
        }
        else
        {
            Debug.LogWarning("Note number not found in content dictionary."); // Log a warning if the key number is not found.
        }
    }

    public void DeselectKey() // Deselects the currently selected key.
    {
        keys[currentlySelectedKey].fontStyle = FontStyles.Normal; // Reset the font style of the previously selected key.
        keyContent.text = defaultText; // Reset the content display to default text.
        currentlySelectedKey = -1; // Mark that no key is currently selected.
    }
}

