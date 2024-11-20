using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : PickUpItems
{
    protected virtual int HealthValue => 50;

    public override void UseItem()
    {
        Debug.Log("Medkit used");
    }
}
