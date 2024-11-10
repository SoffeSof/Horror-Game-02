using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    //This needs to inheret from Item
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
        inventoryManager.Additem(battery);
        Debug.Log("Battery placed in inventory");
        Destroy(gameObject);
    }


}
