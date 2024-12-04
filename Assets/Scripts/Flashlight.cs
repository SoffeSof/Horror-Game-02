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

    //Monster detection variables
    private float flashlightRange = 30f; // Range of the flashlight's effect
    public LayerMask monsterLayer; // Set to only detect monsters
    public float flashlightRadius = 1f;  // The radius of the sphere cast
    private float detectionTimer = 0f; // Timer to track how long the monster is in the light
    private float requiredDetectionTime = 3f; // Time in seconds for the monster to be in the light to respawn


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
        ToggleFlashlight();
        if (isOn)
        {
            Debug.Log("Detecting Monster");
            DetectMonster();
        }
    }

    private void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && Energy > 0) // Toggle flashlight, restrain to only be able to turn on when energy is above 0
        {
            if (isOn == true)
            {
                isOn = false;
                flashlight.enabled = false;
                flickerSound.Stop();
            }
            else
            {
                isOn = true;
                flashlight.enabled = true;

            }
            clickSound.Play();
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
        if (isOn && Energy <= 10f)
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
        if (Energy > 10f) // Stop flickering if flashlight is above 30
        {
            flickerSound.Stop();
        }
    }

    private void DetectMonster()
    {
        // Get the camera's position and direction
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Debug.Log("1");
        // Perform the sphere cast
        if (Physics.SphereCast(cameraPosition, flashlightRadius, cameraForward, out RaycastHit hit, flashlightRange, monsterLayer))
        {
            Debug.Log("2");
            // Check if the sphere cast hits an object tagged as "Monster"
            if (hit.collider.CompareTag("Monster"))
            {
                Debug.Log("Within Range");
                // If the monster is detected, start counting time
                detectionTimer += Time.deltaTime;

                // If the detection time reaches the required time, respawn the monster
                if (detectionTimer >= requiredDetectionTime)
                {
                    // Call the monster's respawn function
                    hit.collider.GetComponent<MonsterAI>().RespawnMonster();
                    
                    // Reset the detection timer after respawn
                    detectionTimer = 0f;
                }
            }
        }
        else
        {
            // Reset the timer if the monster is no longer in the flashlight's range
            detectionTimer = 0f;
        }
    }
       
}


