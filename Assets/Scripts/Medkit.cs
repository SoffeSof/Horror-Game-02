using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item medkit;

    public void AddToInventory()
    {
        bool isInventoryFull = inventoryManager.AddItem(medkit);
        if (isInventoryFull == false)
        {
            Debug.Log("Battery placed in inventory");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }
}
