using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image; // The image component of the slot
    public Color defaultColor; // The default color of the slot when not selected.
    public Color highlightColor; // The color used to highlight the slot when selected.

    public void Awake()
    {
        Deselect(); // Initialize the slot with its default appearance by deselecting it.
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) // Check if the slot is empty (no child objects in this slot).
        {
            GameObject droppedItem = eventData.pointerDrag; // The item being dragged and dropped.
            InventoryItem inventoryItem = droppedItem.GetComponent<InventoryItem>(); // Get the InventoryItem component from the dropped object.
            inventoryItem.parentAfterDrag = transform; // Set this slot as the new parent of the dropped item.
        }
        
    }

    public void Select()
    {
        image.color = highlightColor; // Change the slot's color to indicate it is selected.
    }

    public void Deselect()
    {
        image.color = defaultColor; // Reset the slot's color to indicate it is deselected.
    }
}
