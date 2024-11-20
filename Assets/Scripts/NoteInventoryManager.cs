using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteInventoryManager : MonoBehaviour
{
    // An array of TextMeshProUGUI components for displaying the notes
    public TMP_Text[] notes;
    public Button[] noteButtons;
    public TMP_Text noteContent;
    public Transform notesParent;

    private Dictionary<int, string> noteContents = new Dictionary<int, string>()
{
    { 1, "This is the content for Note 1" },
    { 2, "This is the content for Note 2" },
    { 3, "This is the content for Note 3" },
    { 4, "This is the content for Note 4" },
    { 5, "This is the content for Note 5" },
    { 6, "This is the content for Note 6" }
};

    void Start()
    {
        // Get all TMP_Text components only under the specific parent
        notes = notesParent.GetComponentsInChildren<TMP_Text>();
        noteButtons = notesParent.GetComponentsInChildren<Button>();

        // Set all note texts to "???"
        foreach (TMP_Text note in notes)
        {
            if (note != null) // Ensure the note is not null to avoid errors
            {
                note.text = "???";
            }
        }

        foreach (Button button in noteButtons) //Get all buttons under the parent
        {
            if (button != null) // Ensure the note is not null to avoid errors
            {
                button.interactable = false; // Disable all buttons initially
            }
        }
    }

    // Method to add a note based on its number
    public void AddNoteToInventory(string noteName, int noteNumber)
    {
        Debug.Log(noteNumber);
        Debug.Log(notes.Length);
        // Ensure the noteNumber is within the bounds of the array
        if (noteNumber >= 0 && noteNumber < notes.Length)
        {
            notes[noteNumber].text = noteName; // Update the text component

            // Enable the corresponding button
            if (noteNumber >= 0 && noteNumber < noteButtons.Length)
            {
                noteButtons[noteNumber].interactable = true; // Enable the button for the note
            }
        }
        else
        {
            Debug.LogWarning("Invalid note number"); 
        }
    }

    // New method to set the note content based on noteNumber
    public void SetNoteContent(int noteNumber)
    {
        // Check if the note number exists in the content dictionary
        if (noteContents.ContainsKey(noteNumber))
        {
            string content = noteContents[noteNumber];

            // Set the note content (this could be a UI text field, etc.)
            noteContent.text = content;
        }
        else
        {
            Debug.LogWarning("Note number not found in content dictionary.");
        }
    }
}

