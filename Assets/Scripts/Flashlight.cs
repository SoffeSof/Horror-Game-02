using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool isOn = true; // Indicates if the flashlight is turned on or off
    public Light flashlight; // Reference to the flashlight's Light component
    public AudioSource clickSound; // Sound played when the flashlight is toggled
    public AudioSource flickerSound; // Sound played during flashlight flickering
    //public GameObject flashlightObject; // Reference to the flashlight's game object (not used in the code)

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
                _energy = Mathf.Clamp(value, 0f, 100f); // Ensure energy is between 0 and 100
                HUDController.Instance.UpdateBatteryText(); // Update the HUD to show current battery level everytime Energy is changed
            }
    }
    private float energyDrainRate = 100f/180f; // Energy drain rate per second (100% in 2 minutes)

    // Flickering behavior variables
    public float minTime = 0.1f; // Minimum time between flickers
    public float maxTime = 0.2f; // Maximum time between flickers
    public float Timer; // Timer for controlling flicker timing

    void Start()
    {
        Timer = Random.Range(minTime, maxTime); // Set a random flicker time between minTime and maxTime
        flickerSound.loop = true; // Ensure flicker sound plays in a loop when flickering
        Energy = 100f; // Set initial energy to full
    }
    
    void Update()
    {
        DrainEnergy(); // Drain energy over time while flashlight is on
        LightFlicker(); // Handle flickering effect when energy is low
        ToggleFlashlight(); // Toggle flashlight on or off when the player presses 'F'
        
        if (isOn) // If the flashlight is on, detect monsters
        {
            DetectMonster(); // Detect if any monsters are within flashlight range
        }
    }

    private void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && Energy > 0) // Toggle flashlight, restrain to only be able to turn on when energy is above 0
        {
            if (isOn == true) //If flashlight is on
            {
                isOn = false; // Set bool isOn to false
                flashlight.enabled = false; // Disable flashlight light
                flickerSound.Stop(); // Stop the flicker sound
            }
            else
            { 
                isOn = true; // Set bool isOn to true
                flashlight.enabled = true; // Enable flashlight light

            }
            clickSound.Play(); // Play the click sound when toggling the flashlight
        }
    }

    private void DrainEnergy()
    {
        if (isOn) // Drain flashlight energy over time while flashlight is on
        {
            Energy -= energyDrainRate * Time.deltaTime; // Decrease energy based on the drain rate
            if (Energy <= 0) // If energy runs out
            {
                Energy = 0; // Set energy to 0
                flashlight.enabled = false; // Turn off the flashlight
                isOn = false; // Set bool isOn to false
                flickerSound.Stop(); // Stop the flicker sound
            }
        }
    }

    private void LightFlicker() // Handle flashlight flickering when energy is low
    {
        if (isOn && Energy <= 10f) // Only start flickering if the flashlight is on and energy is below 10
        {
            Timer -= Time.deltaTime; // Countdown only if flashlight is on
            if (Timer <= 0) //If timer goes to 0
            {
                flashlight.enabled = !flashlight.enabled; // Toggle the flashlight state
                Timer = Random.Range(minTime, maxTime); // Reset the timer to a new random value
            }
            if (!flickerSound.isPlaying) // If the flicker sound is not playing, start it
            {
                flickerSound.Play(); // Play the flicker sound
            }
        }
        if (Energy > 10f) // Stop flickering if flashlight is above 10
        {
            flickerSound.Stop(); // Stop the flicker sound
        }
    }

    private void DetectMonster()  // Detect if a monster is within the flashlight's range
    {
        Vector3 cameraPosition = Camera.main.transform.position; // Get camera position
        Vector3 cameraForward = Camera.main.transform.forward; // Get the direction the camera is facing
        if (Physics.SphereCast(cameraPosition, flashlightRadius, cameraForward, out RaycastHit hit, flashlightRange, monsterLayer)) // Perform the sphere cast to detect monsters within the flashlight's range
        {
            if (hit.collider.CompareTag("Monster")) // Check if the sphere cast hits an object tagged as "Monster"
            {
                detectionTimer += Time.deltaTime; // If the monster is detected, start  counting the detection time

                if (detectionTimer >= requiredDetectionTime)  // If the detection time reaches the required threshold, respawn the monster
                {
                    hit.collider.GetComponent<MonsterAI>().RespawnMonster(); // Respawn the monster
                    detectionTimer = 0f;// Reset the detection timer after respawn
                }
            }
        }
        else
        {
            detectionTimer = 0f; // Reset the timer if the monster is no longer in the flashlight's range
        }
    }
       
}


