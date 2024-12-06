using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteInventoryManager : MonoBehaviour
{
    public TMP_Text[] notes; // An array of TextMeshProUGUI components for displaying the notes
    public Button[] noteButtons; // Buttons associated with the notes
    public TMP_Text noteContent; // Text field for displaying the content of the selected note
    public Transform notesParent; // The parent that holds the notes and buttons
    private string defaultText = "No note selected"; // Default text when no note is selected

    // Serialized fields to define the content of the notes
    [SerializeField] string noteConent1;
    [SerializeField] string noteConent2;
    [SerializeField] string noteConent3;
    [SerializeField] string noteConent4;
    [SerializeField] string noteConent5;
    [SerializeField] string noteConent6;

    [SerializeField] private float _notesCollected = 0f; // Private backing field for the notes collected

    public float NotesCollected // Public property for accessing and modifying notes collected. Single source of truth principle
    {
        get { return _notesCollected; }
        set {
                _notesCollected = Mathf.Clamp(value, 0f, 6f); // Ensure the notes collected stays between 0 and 6
                HUDController.Instance.UpdateNotesCollected(); // Update the HUD with the new value
            }
    }

    //Colors
    public Color unlockedColor; // Color for the unlocked notes txt.

    private int currentlySelectedNote = -1;  //Variable to track the currently selected note (initialized to -1 for no selection)

    private Dictionary<int, string> noteContents = new Dictionary<int, string>() //Dictionary for storing the content of each note by note number (0-indexed)
{
    { 0, "Daniel, If you are reading this, you’ve done what I begged you to—erased your memory. You couldn’t bear the weight of your guilt, and neither could I. The curse… it began with what you unleashed. I can’t explain everything yet—it’s too dangerous to remember all at once. But you must understand: this house, this curse, it’s your doing." },
    { 1, "Daniel, You must keep moving. Something is watching you. It’s not alive, not in the way we understand. It is bound to you, feeding on your fear, your regret. You summoned it the night you made the pact. Do you remember the ritual? No, of course you don’t. I made sure of that. But it remembers. It will not stop until it has you. Look for the green light" },
    { 2, "Daniel, Her name was Elise. You loved her, more than life itself. But life wasn’t enough, was it? You thought you could bring her back. You thought the ritual would save her. Instead, you summoned… something else. Something older, something vengeful. Elise is gone, Daniel. You must accept that. What you brought into this world, though—that remains." },
    { 3, "Daniel, The artifact is your only hope. You hid it somewhere in this house, though even I can’t recall where. You sealed it away because you were afraid of what would happen if you used it. But without it, you cannot break the curse. It’s a terrible thing, Daniel. It holds the truth you tried so desperately to escape. And the truth… it will destroy you." },
    { 4, "Daniel, You erased your memory to escape the guilt, but it festers in the dark corners of this house. The thing that follows you is drawn to it. Every choice you made, every life you sacrificed in your desperate attempt to save Elise—it remembers. You’ve hidden from the truth for long enough. To survive, you must face what you’ve done." },
    { 5, "Daniel, This is the final step. The artifact holds the truth—the ritual, the blood, the pact you made with the shadows. When you see it, you will understand why I begged you to forget. But it is not too late. You must escape the house and destroy the artifact before it finds you. The nightmare is closing in, Daniel, and if you fail, there will be no way out." }
};

    void Start()
    {
        NotesCollected = 0; // Initialize notes collected to 0
        notes = notesParent.GetComponentsInChildren<TMP_Text>(); // Get all TMP_Text components under the notes parent
        noteButtons = notesParent.GetComponentsInChildren<Button>(); // Get all buttons under the notes parent

        foreach (TMP_Text note in notes) // FOreach note text component in the array
        {
            if (note != null) // Ensure the note is not null to avoid errors
            {
                note.text = "???"; // Set the default text for locked notes
            }
        }

        foreach (Button button in noteButtons) // For each button in the array
        {
            if (button != null) // Ensure the note is not null to avoid errors
            {
                button.interactable = false; // Buttons are not interactable until notes are unlocked
            }
        }
    }

    public void AddNoteToInventory(string noteName, int noteNumber) // Method to add a note based on its number
    {
        if (noteNumber >= 0 && noteNumber < notes.Length) // Ensure the noteNumber is within the bounds of the array
        {
            notes[noteNumber].text = noteName; // Update the text component
            notes[noteNumber].color = unlockedColor; // Change the color of the text on the buttons.

            if (noteNumber >= 0 && noteNumber < noteButtons.Length) //If the note number is within the bounds of the button array
            {
                noteButtons[noteNumber].interactable = true; // Enable the button for the note
            }

            NotesCollected++; // Increment the number of notes collected
        }
        else
        {
            Debug.LogWarning("Invalid note number"); // Log a warning if the note number is invalid
        }
    }

    public void SelectNote(int noteNumber)  // Method to select a note and display its content
    {
        if (currentlySelectedNote == noteNumber) // If the selected note is the same as the current
        {
            DeselectNote(); //deselct the note
            return; // Exit the method
        }

        if (currentlySelectedNote != -1 && currentlySelectedNote < notes.Length) // If a different note is currently selected and it is within the bounds of the array
        {
            notes[currentlySelectedNote].fontStyle = FontStyles.Normal; // Restore the normal font style
        }

        currentlySelectedNote = noteNumber; // Update the currently selected note

        if (noteContents.ContainsKey(currentlySelectedNote)) // Check if the selected note exists in the dictionary
        {
            noteContent.text = noteContents[currentlySelectedNote]; // Display the content of the selected note
            notes[currentlySelectedNote].fontStyle = FontStyles.Bold; // Make text bold for selected note
        }
        else
        {
            Debug.LogWarning("Note number not found in content dictionary."); // Log a warning if the note number is not found
        }
    }

    public void DeselectNote() // Method to deselect the currently selected note
    {
        notes[currentlySelectedNote].fontStyle = FontStyles.Normal; // Reset font style to normal
        noteContent.text = defaultText; // Clear the note content display
        currentlySelectedNote = -1; // No note is selected
    }
}

