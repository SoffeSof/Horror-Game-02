using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public NoteInventoryManager noteInventoryManager;
    [SerializeField] private int noteNumber;
    [SerializeField] private string noteName;

    public void AddToInventory()
    {
        noteInventoryManager.AddNoteToInventory(noteName, noteNumber);
        Destroy(gameObject);
    }
}
