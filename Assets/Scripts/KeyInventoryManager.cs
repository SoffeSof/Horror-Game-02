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

    //Colors
    public Color unlockedColor;

    private string defaultText = "No key selected"; // Default text for the key content display

     // Variable to track the currently selected key
    private int currentlySelectedKey = -1;

    private Dictionary<int, string> keyContents = new Dictionary<int, string>() //Using 0-based index
{
    { 0, "This is the content for Key 1" },
    { 1, "This is the content for Key 2" },
    { 2, "This is the content for Key 3" },
    { 3, "This is the content for Key 4" },
    { 4, "This is the content for Key 5" },
    { 5, "This is the content for Key 6" }
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

