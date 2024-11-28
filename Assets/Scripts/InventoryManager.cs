using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public GameObject inventoryPanel;
    public GameObject noteInventory;
    public GameObject keyInventory;
    public GameObject itemInventory;
    public MouseLook mouseLook;

    private int selectedSlotIndex = -1;

    private void Update()
    {
        // Check for the "I" key press to toggle inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        if (Input.GetMouseButtonDown(1) && selectedSlotIndex > -1) // 1 is the right mouse button
        {
            UseSelectedItem();
        }

        CheckForSlotChange();
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
            noteInventory.SetActive(false);
            keyInventory.SetActive(false);
            itemInventory.SetActive(true);
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

    private void ChangeSelectedSlot(int newSlotIndex)
    {
        // If the new slot is the same as the current selected slot, deselect it
        if (selectedSlotIndex == newSlotIndex)
        {
            inventorySlots[selectedSlotIndex].Deselect();
            selectedSlotIndex = -1; // No slot is selected
        }
        else
        {
            // If there was a previously selected slot, deselect it
            if (selectedSlotIndex >= 0)
            {
                inventorySlots[selectedSlotIndex].Deselect();
            }
            
            // Select the new slot and update the selectedSlotIndex
            inventorySlots[newSlotIndex].Select();
            selectedSlotIndex = newSlotIndex;
        }
    }

    private void CheckForSlotChange()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int slotIndex);
            if (isNumber && slotIndex > 0 && slotIndex < 8)
            {
                ChangeSelectedSlot(slotIndex - 1);
            }
        }
    }

    private void UseSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlotIndex];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            itemInSlot.count--;
            PickUpItems itemScript = itemInSlot.GetComponent<PickUpItems>();
            if (itemScript != null)
            {
                itemScript.UseItem();
            }
            //Code that calls UseItem() method from the correct script script
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.UpdateCount();
                }
        }
    }
}
