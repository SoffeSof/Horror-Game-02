using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool isOn = true;
    public Light flashlight;
    public AudioSource clickSound;
    public AudioSource flickerSound;
    public GameObject flashlightObject;

    [SerializeField] private float _energy = 100f; // Private backing field

    public float Energy
    {
        get { return _energy; }
        set { 
                _energy = Mathf.Clamp(value, 0f, 100f);
                HUDController.Instance.UpdateBatteryText();
            } // Clamps the value between 0 and 100
    }
    private float energyDrainRate = 100f/180f; // Energy drain rate of the flashlight per second, 2 minutes to drain a flashlight

    //Flickering variables
    public float minTime = 0.1f;
    public float maxTime = 0.2f;
    public float Timer;

    void Start()
    {
        Timer = Random.Range(minTime, maxTime);
        flickerSound.loop = true;
        Energy = 100f;
    }
    
    void Update()
    {
        DrainEnergy();
        LightFlicker();

        if (Input.GetKeyDown(KeyCode.F) && Energy > 0) // Toggle flashlight, restrain to only be able to turn on when energy is above 0
        {
            if (isOn == true)
            {
                isOn = false;
                flashlight.enabled = false;
                flickerSound.Stop();
                clickSound.Play();
            }
            else
            {
                isOn = true;
                flashlight.enabled = true;
                clickSound.Play();
            }
        }
    }

    private void DrainEnergy()
    {
        if (isOn)
        {
            Energy -= energyDrainRate * Time.deltaTime;
            if (Energy <= 0)
            {
                Energy = 0;
                flashlight.enabled = false;
                isOn = false;
                flickerSound.Stop();
            }
        }
    }

    private void LightFlicker()
    {
        // Only start flickering if the flashlight is on and energy is below 10
        if (isOn && Energy <= 30f)
        {
            Timer -= Time.deltaTime; // Countdown only if flashlight is on
            if (Timer <= 0)
            {
                flashlight.enabled = !flashlight.enabled; // Toggle the flashlight state
                Timer = Random.Range(minTime, maxTime); // Reset the timer to a new random value
            }
            if (!flickerSound.isPlaying)
            {
                flickerSound.Play();
            }
        }
        if (Energy > 30f) // Stop flickering if flashlight is above 30
        {
            flickerSound.Stop();
        }
    }
       
}


