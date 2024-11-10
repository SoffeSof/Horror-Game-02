using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : PickUpItems
{
    [SerializeField] private float energyValue = 20f;
    public Flashlight flashlight;

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

}
