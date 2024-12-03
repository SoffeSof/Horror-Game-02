using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : PickUpItems
{
    [SerializeField] private float energyValue = 20f;
    public Flashlight flashlight;

    protected override void Awake()
    {
        flashlight = FindObjectOfType<Flashlight>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void ChargeFlashlight()
    {
        if (flashlight.Energy + energyValue > 100)
        {
            AddToInventory();
        }
        else
        {
            flashlight.Energy += energyValue;
            Destroy(gameObject);
        }
    }

    public override void UseItem()
    {
        //maybe add a check to see if the flashlight is already full and then ask if they are sure, they want to use the item, if it is.
        Debug.Log("Battery used"); 
        flashlight.Energy += energyValue;
    }

}
