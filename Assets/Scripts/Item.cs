using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    //https://www.youtube.com/watch?v=oJAE6CbsQQA watch this video to understand how to make items interactable

    public Sprite image;
    public itemType type;
    public bool isStackable = true;
    public int maxStackSize = 1;
    

    public enum itemType
    {
        Battery,
        Key,
        Note,
        Medkit,
        Pills
    }

}
