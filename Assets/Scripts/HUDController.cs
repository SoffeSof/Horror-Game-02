using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    // Static instance that can be accessed from other scripts / Singleton
    public static HUDController Instance { get; private set; }
    [SerializeField] private InventoryManager inventoryManager;


    private void Awake()
    {
        // Check if the instance already exists and destroy this one if it does
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Set the instance to this and make it persist across scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private string textInFront = "[E] ";

    public void EnableInteractionText(string text, Item item)
    {
        Debug.Log("2");
        bool isInventoryFull = inventoryManager.CheckInventorySpace(item);
        if (isInventoryFull)
        {
            interactionText.text = "Inventory is full";
            Debug.Log("3");
        }
        else //There needs to be implemented logic in the CheckForInventory space that items can be stackable, and just because the inventory is full, it doesn't mean that the item can't be picked up and added to a stack
        {
            interactionText.text = textInFront + text;
        }
        interactionText.gameObject.SetActive(true);

    }

    public void DisableInteractableText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
