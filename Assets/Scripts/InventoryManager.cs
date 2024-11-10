using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public GameObject inventoryPanel;
    public MouseLook mouseLook;

    private void Update()
        {
            // Check for the "I" key press to toggle inventory
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
        }

    public void Additem(Item item) //finds the first empty slot and adds the item to it
    {
        //Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                //Spawn new item
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);    
    }

    public void ToggleInventory()
    {
        if (mouseLook.isInventoryOpen == false)
        {
            // Open the inventory: show panel and unlock cursor
            Cursor.lockState = CursorLockMode.None;
            inventoryPanel.SetActive(true);
            mouseLook.isInventoryOpen = true;
        }
        else
        {
            // Close the inventory: hide panel and lock cursor
            inventoryPanel.SetActive(false);
            mouseLook.isInventoryOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
