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

    public bool AddItem(Item item) //finds the first empty slot and adds the item to it
    {
        //Check if any slot has the same item with count lower than maxStack
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.maxStackSize && itemInSlot.item.isStackable == true)
            {
                //Add count to the item stack
                itemInSlot.count++;
                itemInSlot.UpdateCount();

                return false;
            }
        }

        //Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                //Spawn new item
                SpawnNewItem(item, slot);
                return false;
            }
        }

        return true;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform); //GO = Game Object
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

    public bool CheckInventorySpace(Item item) //finds the first empty slot and adds the item to it
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.maxStackSize && itemInSlot.item.isStackable == true)
            {
                //If there are any slots with the same type of item and the count is less than maxStack and is stackable, return false
                return false;
            }
        }
        //Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                //if there is an empty slot, return false
                return false;
            }
        }

        return true; //if there are no empty slots and no available stacks, return true
    }

}
