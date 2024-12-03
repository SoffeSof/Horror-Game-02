using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item itemType;

    protected virtual void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();	
    }

    public virtual void AddToInventory()
    {
        bool isInventoryFull = inventoryManager.AddItem(itemType);
        if (isInventoryFull == false)
        {
            Debug.Log($"{itemType.name} placed in inventory");
            Destroy(gameObject);  // Destroys the item from the scene once it is added to the inventory
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    public virtual void UseItem()
    {
        Debug.Log("Item used");
    }
}
