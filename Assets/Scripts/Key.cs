using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Door door;
    public Interactable doorInteractable;
    public Interactable interactable;
    public KeyInventoryManager keyInventoryManager;

    public string keyName;
    public int keyNumber;

    public void Awake()
    {
        interactable = GetComponent<Interactable>();
        keyInventoryManager = FindObjectOfType<KeyInventoryManager>();

        interactable.displayMessage = "Pick up Key";

         if (door != null)
        {
            doorInteractable = door.interactable; // Link the door's interactable
        }
        else
        {
            Debug.LogError("Key does not have a door linked to it");
        }
    }
    
    public void AddToInventory()
    {
        keyInventoryManager.AddNoteToInventory(keyName, keyNumber);
        door.hasKey = true;
        door.SetUnlockDisplayMessage();
        Destroy(gameObject);
    }
}
