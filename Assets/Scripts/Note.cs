using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteInventoryManager noteInventoryManager; // Reference to the NoteInventoryManager to manage note inventory
    [SerializeField] private int noteNumber; // The number identifier for the note
    [SerializeField] private string noteName; // The name of the note
    
    public void AddToInventory() // Method to add the note to the inventory
    {
        noteInventoryManager.AddNoteToInventory(noteName, noteNumber); // Adds the note with its name and number to the inventory
        Destroy(gameObject); // Destroys the note object in the scene after adding it to the inventory
    }
}
