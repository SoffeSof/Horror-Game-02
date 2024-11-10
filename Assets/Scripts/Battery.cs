using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] private float energy = 20f;
    public Flashlight flashlight;
    public InventoryManager inventoryManager;
    public Item battery;
    public void ChargeFlashlight()
    {
        if (flashlight.Energy + energy > 100)
        {
            AddToInventory();
        }
        else
        {
            flashlight.Energy += energy;
            Destroy(gameObject);
        }
    }

    public void AddToInventory()
    {
        bool isInventoryFull = inventoryManager.AddItem(battery);
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
