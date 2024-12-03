using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : PickUpItems
{
    public LeverBase leverBase;
    
    protected override void Awake()
    {
        leverBase = FindObjectOfType<LeverBase>();
        inventoryManager = FindObjectOfType<InventoryManager>();	
    }

    public override void AddToInventory()
    {
        bool isInventoryFull = inventoryManager.AddItem(itemType);
        if (isInventoryFull == false)
        {
            Debug.Log($"{itemType.name} placed in inventory");
            leverBase.AddLever();
            Destroy(gameObject);  // Destroys the item from the scene once it is added to the inventory
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }
}
