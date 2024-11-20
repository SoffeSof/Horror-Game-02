using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color defaultColor;
    public Color highlightColor;

    public void Awake()
    {
        Deselect();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject droppedItem = eventData.pointerDrag;
            InventoryItem inventoryItem = droppedItem.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform; 
        }
        
    }

    public void Select()
    {
        image.color = highlightColor;
    }

    public void Deselect()
    {
        image.color = defaultColor;
    }
    
}
