using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyInventoryManager : MonoBehaviour
{
    // An array of TextMeshProUGUI components for displaying the notes
    public TMP_Text[] keys;
    public Button[] keyButtons;
    public TMP_Text keyContent;
    public Transform keysParent;

    public int keyCount = 0;

    //Colors
    public Color unlockedColor;

    private string defaultText = "No key selected"; // Default text for the key content display

     // Variable to track the currently selected key
    private int currentlySelectedKey = -1;

    private Dictionary<int, string> keyContents = new Dictionary<int, string>() //Using 0-based index
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
        // Get all TMP_Text components only under the specific parent
        keys = keysParent.GetComponentsInChildren<TMP_Text>();
        keyButtons = keysParent.GetComponentsInChildren<Button>();

        // Set all note texts to "???"
        foreach (TMP_Text key in keys)
        {
            if (key != null) // Ensure the note is not null to avoid errors
            {
                key.text = "???";
            }
        }

        foreach (Button button in keyButtons) //Get all buttons under the parent
        {
            if (button != null) // Ensure the note is not null to avoid errors
            {
                button.interactable = false; // Disable all buttons initially
            }
        }
    }

    // Method to add a key based on its number
    public void AddNoteToInventory(string keyName, int keyNumber)
    {
        Debug.Log("Adding key " + keyName + " to inventory");
        // Ensure the keyNumber is within the bounds of the array
        if (keyNumber >= 0 && keyNumber < keys.Length)
        {
            keys[keyNumber].text = keyName; // Update the text component
            keys[keyNumber].color = unlockedColor; // Change the color of the text
            keyCount++; // Increment the key count

            // Enable the corresponding button
            if (keyNumber >= 0 && keyNumber < keyButtons.Length)
            {
                keyButtons[keyNumber].interactable = true; // Enable the button for the note
            }
        }
        else
        {
            Debug.LogWarning("Invalid key number"); 
        }
    }

    // New method to set the note content based on noteNumber
    public void SelectKey(int keyNumber)
    {
        // If the same note is clicked again, unselect it
        if (currentlySelectedKey == keyNumber)
        {
            DeselectKey();
            return;
        }

         // If a different note is currently selected, reset its color
        if (currentlySelectedKey != -1 && currentlySelectedKey < keys.Length)
        {
            keys[currentlySelectedKey].fontStyle = FontStyles.Normal;
        }
        
        // Update the selection
        currentlySelectedKey = keyNumber;

        // Check if the note number exists in the content dictionary
        if (keyContents.ContainsKey(currentlySelectedKey))
        {
            // Set the note content (this could be a UI text field, etc.)
            keyContent.text = keyContents[currentlySelectedKey];
            keys[currentlySelectedKey].fontStyle = FontStyles.Bold; // Make text bold for selected note
        }
        else
        {
            Debug.LogWarning("Note number not found in content dictionary.");
        }
    }

    public void DeselectKey()
    {
        keys[currentlySelectedKey].fontStyle = FontStyles.Normal;
        keyContent.text = defaultText; // Clear the content display
        currentlySelectedKey = -1; // No note is selected
    }
}

