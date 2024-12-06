using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : PickUpItems // Represents a lever that can be picked up and added to inventory. Inherits from PickUpItems class
{
    public LeverBase leverBase;  // Reference to the LeverBase object
    
    protected override void Awake()
    {
        leverBase = FindObjectOfType<LeverBase>(); // Find and assign the LeverBase object in the scene.
        inventoryManager = FindObjectOfType<InventoryManager>(); // Find and assign the InventoryManager object in the scene.	 
    }

    public override void AddToInventory() // Called to add the lever to the player's inventory.
    {
        bool isInventoryFull = inventoryManager.AddItem(itemType); // Try to add the lever item to the inventory.
        if (isInventoryFull == false) // If the inventory is not full.
        {
            leverBase.AddLever(); // Update the LeverBase to reflect the addition of the lever.
            Destroy(gameObject);  // Destroy the lever object from the scene, as it has been added to the inventory.
        }
        else
        {
            Debug.Log("Inventory is full"); // Log a message if the inventory is full and the lever cannot be added.
        }
    }
}
