using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots; // Array of inventory slots
    public GameObject inventoryItemPrefab; // Prefab used for creating inventory items

    public GameObject inventoryPanel; // The main inventory UI panel
    public GameObject noteInventory; // Panel for note-related items
    public GameObject keyInventory; // Panel for key-related items
    public GameObject itemInventory; // Panel for generic items
    public MouseLook mouseLook; // Reference to mouse look script for cursor locking/unlocking

    private int selectedSlotIndex = -1; // Currently selected inventory slot (-1 = none)
    private string cantBeUsedString = "Item cant be used here"; // Message for unusable items

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // Check if the "I" key is pressed
        {
            ToggleInventory(); // Toggle the inventory panel's visibility
        }

        if (Input.GetMouseButtonDown(1) && selectedSlotIndex > -1) // Check if right mouse button is pressed and a slot is selected
        {
            UseSelectedItem(); // Attempt to use the item in the selected slot
        }

        CheckForSlotChange(); // Check if the player changes the selected inventory slot via number keys
    } 
    
    public bool AddItem(Item item) // Method to add an item to the inventory
    {
        // First, try to add the item to an existing stack. Check if any slot has the same item with count lower than maxStack
        for (int i = 0; i < inventorySlots.Length; i++) // Loop through all inventory slots
        {
            InventorySlot slot = inventorySlots[i]; // Get the current slot
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();  // Get the item in the current slot, if any
            if (itemInSlot != null && itemInSlot.item == item // Check if the slot has the same item
            && itemInSlot.count < item.maxStackSize  // Check if the stack is not full
            && itemInSlot.item.isStackable == true) // Check if the item is stackable
            {
                itemInSlot.count++; // Increment stack count
                itemInSlot.UpdateCount(); // Update the UI to reflect the new count
                return false; // Indicate that the item was successfully added to an existing stack and therefore, the inventory was not full
            }
        }

        // If no stack is available, try to add the item to an empty slot
        for (int i = 0; i < inventorySlots.Length; i++) // Loop through all inventory slots again
        {
            InventorySlot slot = inventorySlots[i]; // Get the current slot
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); // Get the item in the current slot, if any
            if (itemInSlot == null) // Check if the slot is empty
            {
                //Spawn new item
                SpawnNewItem(item, slot); // Create a new item in the empty slot
                return false; // Indicate that the item was successfully added to an empty slot and therefore, the inventory was not full
            }
        }

        return true;// If no empty slots or stackable slots were found, inventory is full and return true
    }

    public void SpawnNewItem(Item item, InventorySlot slot) // Method to create and place a new item in a slot
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform); // Instantiate the item prefab as a child of the slot
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();  // Get the InventoryItem component from the prefab
        inventoryItem.InitialiseItem(item); // Initialize the item with its properties (e.g., name, icon)    
    }

    public void ToggleInventory()  // Method to show or hide the inventory
    {
        if (mouseLook.isInventoryOpen == false) // Check if the inventory is currently closed
        {
            Cursor.lockState = CursorLockMode.None; //Unlock the cursor so the player can interact with the UI
            inventoryPanel.SetActive(true); // Show the main inventory panel
            mouseLook.isInventoryOpen = true; // Mark the inventory as open
        }
        else
        {
            noteInventory.SetActive(false); // Hide the notes sub-panel
            keyInventory.SetActive(false); // Hide the keys sub-panel
            itemInventory.SetActive(true); // Ensure the generic items panel is shown
            inventoryPanel.SetActive(false); // Hide the main inventory panel
            mouseLook.isInventoryOpen = false; // Mark the inventory as closed
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor for gameplay
        }
    }

    public bool CheckInventorySpace(Item item) // Method to check if there's space for an item in the inventory. called in Hudcontroller, so a text is displayed when poiting at if, if the inventory is full
    {
        for (int i = 0; i < inventorySlots.Length; i++) // Loop through all inventory slots
        {
            InventorySlot slot = inventorySlots[i]; // Get the current slot
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); // Get the item in the current slot, if any
            if (itemInSlot != null && itemInSlot.item == item // Check if the slot has the same item
            && itemInSlot.count < item.maxStackSize // Check if the stack is not full
            && itemInSlot.item.isStackable == true) // Check if the item is stackable
            {
                //If there are any slots with the same type of item and the count is less than maxStack and is stackable, return false
                return false; // Indicate that there is space in an existing stack
            }
        }
        // Check for empty slots
        for (int i = 0; i < inventorySlots.Length; i++) // Loop through all inventory slots again
        {
            InventorySlot slot = inventorySlots[i];// Get the current slot
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); // Get the item in the current slot, if any
            if (itemInSlot == null) // Check if the slot is empty
            {
                //if there is an empty slot, return false
                return false;// Indicate that there is an empty slot
            }
        }

        return true; // If no space in stacks or empty slots, inventory is full and return true
    }

    private void ChangeSelectedSlot(int newSlotIndex) // Method to change the currently selected inventory slot
    {
        if (selectedSlotIndex == newSlotIndex) // Check if the same slot is selected again
        {
            inventorySlots[selectedSlotIndex].Deselect(); // Deselect the slot
            selectedSlotIndex = -1; // Mark no slot as selected
        }
        else // If a different slot is selected
        {
            if (selectedSlotIndex >= 0) // Check if there was a previously selected slot
            {
                inventorySlots[selectedSlotIndex].Deselect(); // Deselect the previously selected slot
            }
            
            // Select the new slot and update the selectedSlotIndex
            inventorySlots[newSlotIndex].Select(); // Highlight the newly selected slot
            selectedSlotIndex = newSlotIndex; // Update the selected slot index
        }
    }

    private void CheckForSlotChange() // Method to detect if the player changes the selected slot using number keys
    {
        if (Input.inputString != null) // Check if a key was pressed
        {
            bool isNumber = int.TryParse(Input.inputString, out int slotIndex); // Attempt to parse the input as a number
            if (isNumber && slotIndex > 0 && slotIndex < 8)  // Check if the number corresponds to a valid slot (1-7)
            {
                ChangeSelectedSlot(slotIndex - 1); // Convert the number to a zero-based index and change the selected slot
            }
        }
    }

    private void UseSelectedItem() // Method to use the item in the currently selected slot
    {
        InventorySlot slot = inventorySlots[selectedSlotIndex]; // Get the currently selected slot
        if (slot == null || slot.GetComponentInChildren<InventoryItem>() == null) // Check if the slot is empty
        {
            return; // Exit the method early if the slot is empty 
        }

        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); // Get the item in the selected slot
        if (itemInSlot.item.cantBeUsed == true) // Check if the item is marked as unusable
        {
            HUDController.Instance.EnableCantBeUsedInteractionText(cantBeUsedString); // Show a warning message
            return;
        }
        if (itemInSlot != null) // Check if the item exists
        {
            itemInSlot.count--; // Decrease the stack count
            PickUpItems itemScript = itemInSlot.GetComponent<PickUpItems>(); // Get the PickUpItems script attached to the item
            if (itemScript != null) // Check if the script exists
            {
                itemScript.UseItem(); // Call the item's specific use behavior
            }
            if (itemInSlot.count <= 0) // Check if the stack is now empty
            {
                Destroy(itemInSlot.gameObject); // Remove the item from the inventory
            }
            else //if there is still items left in the stack
            {
                    itemInSlot.UpdateCount(); // Update the stack count in the UI
            }
        }
    }

    public void RemoveItem(Item itemToRemove) // Method to remove an item from the inventory. Called from LeverBase, when the lever is used.
    {
        for (int i = 0; i < inventorySlots.Length; i++) // Loop through all inventory slots
        {
            InventorySlot slot = inventorySlots[i]; // Get the current slot
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); // Get the item in the current slot, if any

            if (itemInSlot != null && itemInSlot.item == itemToRemove) //Check if the item in the slot is the one needed to be removed
            {
                itemInSlot.count--; // Decrease the stack count
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject); // Remove the item from the inventory
                }
                else
                {
                    itemInSlot.UpdateCount(); // Update the stack count in the UI
                }
            }
        }
    }
    
}
