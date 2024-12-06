using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    public InventoryManager inventoryManager; // Reference to the InventoryManager to manage the inventory
    public Item itemType;  // The type of item this object represents

    protected virtual void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>(); // Find the InventoryManager in the scene and assign it to the reference	 
    }

    public virtual void AddToInventory()
    {
        bool isInventoryFull = inventoryManager.AddItem(itemType); // Check if the item can be added to the inventory and add it through the InventoryManager.AddItem
        if (isInventoryFull == false) // If the item was successfully added to the inventory
        {
            Destroy(gameObject);  // Destroys the item from the scene once it is added to the inventory
        }
        else
        {
            Debug.Log("Inventory is full"); // Log a message if the inventory is full
        }
    }

    public virtual void UseItem() // Method to use the item, gets overridden by specific item types
    {
        Debug.Log("Item used");
    }
}
