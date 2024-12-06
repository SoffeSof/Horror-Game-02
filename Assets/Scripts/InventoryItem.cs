using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler // Interfaces for drag events
{
    [HideInInspector] public Transform parentAfterDrag; // The parent object the item should return to after dragging
    [HideInInspector] public Item item; // The item associated with this inventory object
    public int count = 1; // The count of how many of this item the player has
    public TMP_Text countText; // Text UI element to display the count of the item
    public Image itemImage; // UI image that represents the item

    public void InitialiseItem(Item newItem) // Initializes the item with a new item type and updates its visual representation
    {
         item = newItem; // Assign the new item to this inventory item
        itemImage.sprite = newItem.image; // Set the item's image in the UI
        AddScriptComponent(); // Add the appropriate script component based on the item type
        UpdateCount(); // Update the count display for the item
    }  

    public void UpdateCount() // Updates the display count text and determines if the text should be visible
    {
        countText.text = count.ToString(); // Set the count text to the item's count
        bool textActive = count > 1; // Only show the count if it's greater than 1
        countText.gameObject.SetActive(textActive); // Enable/Disable count text based on count value
    }


    public void OnBeginDrag(PointerEventData eventData) // Called when the player starts dragging the item
    {
        parentAfterDrag = transform.parent; // Store the original parent
        transform.SetParent(transform.root); // Remove the item from its parent to move it independently
        transform.SetAsLastSibling(); // Place it at the last position in the hierarchy
        itemImage.raycastTarget = false; // Disable the image from blocking raycasts while dragging
    }

    public void OnDrag(PointerEventData eventData) // Called while the item is being dragged
    {
        Debug.Log("OnDrag");
        transform.position = Input.mousePosition; // Move the item to follow the mouse position
    }

    public void OnEndDrag(PointerEventData eventData) // Called when the player finishes dragging the item
    {
        Debug.Log("OnEndDrag"); // Logs when the drag ends
        transform.SetParent(parentAfterDrag); // Restore the item's original parent
        itemImage.raycastTarget = true; // Re-enable the image to respond to raycasts
    }

     // Adds a script component to the item based on its type
    private void AddScriptComponent()
    {
        Debug.Log(item.type); // Logs the item type
        switch (item.type)
        {
            case Item.itemType.Battery:
                gameObject.AddComponent<Battery>(); // Add Battery script if item is of type Battery
                Debug.Log("Battery item detected!"); // Log for debugging
                break;
            case Item.itemType.Medkit:
                gameObject.AddComponent<Medkit>(); // Add Medkit script if item is of type Medkit
                Debug.Log("Medkit item detected!"); // Log for debugging
                break;
            case Item.itemType.Pills:
                gameObject.AddComponent<Medkit>(); // Add Medkit script if item is of type Pills
                gameObject.GetComponent<Medkit>().HealthValue = 10; // XSets itds health value to 10
                Debug.Log("Pills item detected!"); // Log for debugging
                break;
            case Item.itemType.Lever:
                gameObject.AddComponent<Lever>(); // Add Lever script if item is of type Lever
                Debug.Log("Lever item detected!"); // Log for debugging
                break;
            case Item.itemType.MagicBall:
                Debug.Log("MagicBall item detected!"); // Log for MagicBall items (no script added here)
                break;
            default:
                Debug.LogWarning("Unhandled item type: " + item.type); // Log a warning if an unhandled item type is encountered
                break;
        }
    }
}
