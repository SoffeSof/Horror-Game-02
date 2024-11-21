using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : PickUpItems
{
    [SerializeField] public int HealthValue = 25;

    public Healthbar healthbar;

    void Start()
    {
        healthbar = FindObjectOfType<Healthbar>();
    }

    public override void UseItem()
    {
        Debug.Log("Using Medkit");
        healthbar.Health += HealthValue;
        Debug.Log("Health increased by " + HealthValue);
    }
}
