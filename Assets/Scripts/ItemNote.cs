using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Note")]
public class ItemNote : Item // Inherits from the base Item class to reuse existing functionality and add note-specific features.
{
    public string noteContent; // The actual text content of the note (e.g., a message or clue for the player).
    public noteNumber number; // An identifier for the specific note (useful for organizing and referencing notes).

    // Enum to define different note identifiers for the game.
    public enum noteNumber
    {
        Note1, // Represents the first note.
        Note2, // Represents the second note.
        Note3, // Represents the third note.
        Note4, // Represents the fourth note.
        Note5, // Represents the fifth note.
        Note6, // Represents the sixth note.
    }
}
//NOT GETTING USED
