using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Note")]
public class ItemNote : Item
{
    public string noteContent;  // New field specific to Note items
    public noteNumber number;  // New field specific to Note items

    public enum noteNumber
    {
        Note1,
        Note2,
        Note3,
        Note4,
        Note5,
        Note6,
        Note7,
        Note8,
        Note9,
    }
}
