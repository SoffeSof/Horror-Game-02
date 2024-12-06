using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    //https://www.youtube.com/watch?v=oJAE6CbsQQA watch this video to understand how to make items interactable

    public Sprite image; // The visual representation of the item, used in the inventory UI.
    public itemType type; // The type of the item, defined by the custom enumeration `itemType`.
    public bool isStackable = true; // Determines if multiple copies of this item can be stacked together in the inventory.
    public int maxStackSize = 1; // The maximum number of items that can be stacked in one inventory slot.
    public bool cantBeUsed = false; // Indicates if this item cannot be used (e.g., placeholder or decorative items).

    // Enum that defines the different item types in the game.
    public enum itemType
    {
        Battery,   // Item type for batteries (e.g., for recharging devices).
        Key,       // Item type for keys (e.g., to unlock doors or chests).
        Note,      // Item type for collectible or readable notes.
        Medkit,    // Item type for healing the player.
        Pills,     // Item type for boosting player stats or healing.
        Lever,     // Item type for interactable levers or switches.
        MagicBall  // Item type for magical or unique objects in the game.
    }

}
