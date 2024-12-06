using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light lightSource; // Reference to the light that will flicker
    public Light otherLight; // Optional reference to another light that will flicker in sync
    public float minTime; // Minimum time interval for light flickering
    public float maxTime; // Maximum time interval for light flickering
    private float timer; // Timer to track the flickering interval

    private void Start()
    {
        lightSource = GetComponent<Light>();  // Get the Light component from the current object
        timer = Random.Range(minTime, maxTime);  // Set the initial timer to a random value between minTime and maxTime
    }

    private void Update()
    {
        // Call the Flickering method every frame to update the light behavior
    }

    private void Flickering()
    {
        if (timer > 0) // Check if there is still time left on the timer
        {
            timer -= Time.deltaTime; // Decrease the timer by the time elapsed since the last frame
        }
        else if (timer <= 0) // If the timer reaches zero
        {
            lightSource.enabled = !lightSource.enabled;  // Toggle the main light on/off
            if (otherLight != null)  // If there is a second light assigned
            {   
                otherLight.enabled = !otherLight.enabled;  // Toggle the second light on/off
            }
            timer = Random.Range(minTime, maxTime);  // Reset the timer to a new random value between minTime and maxTime
        }
    }
}
