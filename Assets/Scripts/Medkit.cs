using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : PickUpItems
{
    [SerializeField] public int HealthValue = 25;

    public override void UseItem()
    {
        HUDController.Instance.Sanity += HealthValue;
    }
}
