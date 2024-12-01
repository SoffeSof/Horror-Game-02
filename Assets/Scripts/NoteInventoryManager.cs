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
    private string defaultText = "No note selected";

    [SerializeField] string noteConent1;
    [SerializeField] string noteConent2;
    [SerializeField] string noteConent3;
    [SerializeField] string noteConent4;
    [SerializeField] string noteConent5;
    [SerializeField] string noteConent6;

    [SerializeField] private float _notesCollected = 100f; // Private backing field

    public float NotesCollected //Single source of truth principle
    {
        get { return _notesCollected; }
        set {
                _notesCollected = Mathf.Clamp(value, 0f, 6f); 
                HUDController.Instance.UpdateNotesCollected();
            }
    }

    //Colors
    public Color unlockedColor;

     // Variable to track the currently selected note
    private int currentlySelectedNote = -1;

    private Dictionary<int, string> noteContents = new Dictionary<int, string>() //Using 0-based index
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
        NotesCollected = 0; // Set the notes collected to the initial value
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
        // Ensure the noteNumber is within the bounds of the array
        if (noteNumber >= 0 && noteNumber < notes.Length)
        {
            notes[noteNumber].text = noteName; // Update the text component
            notes[noteNumber].color = unlockedColor; // Change the color of the text

            // Enable the corresponding button
            if (noteNumber >= 0 && noteNumber < noteButtons.Length)
            {
                noteButtons[noteNumber].interactable = true; // Enable the button for the note
            }

            NotesCollected++;
        }
        else
        {
            Debug.LogWarning("Invalid note number"); 
        }
    }

    // New method to set the note content based on noteNumber
    public void SelectNote(int noteNumber)
    {
        // If the same note is clicked again, unselect it
        if (currentlySelectedNote == noteNumber)
        {
            DeselectNote();
            return;
        }

         // If a different note is currently selected, reset its color
        if (currentlySelectedNote != -1 && currentlySelectedNote < notes.Length)
        {
            notes[currentlySelectedNote].fontStyle = FontStyles.Normal;
        }
        
        // Update the selection
        currentlySelectedNote = noteNumber;

        // Check if the note number exists in the content dictionary
        if (noteContents.ContainsKey(currentlySelectedNote))
        {
            // Set the note content (this could be a UI text field, etc.)
            noteContent.text = noteContents[currentlySelectedNote];
            notes[currentlySelectedNote].fontStyle = FontStyles.Bold; // Make text bold for selected note
        }
        else
        {
            Debug.LogWarning("Note number not found in content dictionary.");
        }
    }

    public void DeselectNote()
    {
        notes[currentlySelectedNote].fontStyle = FontStyles.Normal;
        noteContent.text = defaultText; // Clear the content display
        currentlySelectedNote = -1; // No note is selected
    }
}

