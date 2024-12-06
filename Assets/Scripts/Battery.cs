using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : PickUpItems
{
    [SerializeField] private float energyValue = 20f; // The amount of energy the battery provides when used.
    public Flashlight flashlight; // Reference to the Flashlight object to manipulate its energy.

    protected override void Awake()  //Override of the Awake method from parent class to initialize references.
    {
        // Find and assign the Flashlight and InventoryManager in the scene.
        flashlight = FindObjectOfType<Flashlight>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void ChargeFlashlight() // Method to charge the flashlight or add to inventory when the player picks up the battery.
    {
        if (flashlight.Energy + energyValue > 100) // Check if adding the battery's energy would exceed the flashlight's maximum energy (100).
        {
            AddToInventory(); // If it would exceed, add the battery to the inventory instead of charging the flashlight.
        }
        else
        {
            flashlight.Energy += energyValue; // If it won't exceed, add energy to the flashlight and destroy the battery object.
            Destroy(gameObject); // Destroys the battery object after use.
        }
    }

    public override void UseItem() // Override the UseItem method from PickUpItems to define how the battery is used.
    {
        Debug.Log("Battery used"); 
        flashlight.Energy += energyValue; // Add the battery's energy to the flashlight's energy when used.
    }

}
