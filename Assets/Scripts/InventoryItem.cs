using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public Item item;
    public int count = 1;
    public TMP_Text countText;

    
    public Image itemImage;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        itemImage.sprite = newItem.image;
        AddScriptComponent();
        UpdateCount();
    }  

    public void UpdateCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        itemImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        transform.SetParent(parentAfterDrag);
        itemImage.raycastTarget = true;
    }

    private void AddScriptComponent()
    {
        Debug.Log(item.type);
        switch (item.type)
            {
            case Item.itemType.Battery:
                gameObject.AddComponent<Battery>();
                Debug.Log("Battery item detected!");
                break;
            case Item.itemType.Medkit:
                gameObject.AddComponent<Medkit>();
                Debug.Log("Medkit item detected!");
                break;
            case Item.itemType.Key:
                // Add logic for Key if required
                Debug.Log("Key item detected!");
                break;
            case Item.itemType.Note:
                // Add logic for Note if required
                Debug.Log("Note item detected!");
                break;
            case Item.itemType.Pills:
                gameObject.AddComponent<Pills>();
                Debug.Log("Pills item detected!");
                break;
            default:
                Debug.LogWarning("Unhandled item type: " + item.type);
                break;
            }
    }
}
