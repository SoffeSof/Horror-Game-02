using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Scripts
    public Interactable interactable; // Reference to the interactable component for interaction

    //Door State Variables
    public bool isOpen = false; // Bool to keep track of state of whether the door is open or closed
    public bool isLocked = false; // Whether the door is locked or not
    private bool isAnimating = false;  //Prevents animation while the door is already animating
    public bool hasKey = false; // Whether the player has a key to unlock the door
    public bool cantBeOpened = false; // Whether the door cannot be opened
    public bool shakeDoorOnly = false; // Whether the door can only be shaken (when locked and forced shut)

    //Door Movement Variables
    public float openSpeed = 0.5f; // Speed at which the door opens
    public float closeSpeed = 1.0f; // Speed at which the door closes
    public float openAngle = 90.0f; // Angle the door should open
    public float closeAngle = 0.0f; // Angle the door should close
    public float xAngle = 0.0f; // Rotation along the X-axis (fixed for door orientation)

    //Display messages
    public string openDisplayMessage = "Open Door"; // Message displayed when the door can be opened
    public string closeDisplayMessage = "Close Door"; // Message displayed when the door can be closed
    public string lockedDisplayMessage = "Locked"; // Message displayed when the door is locked
    public string unlockDisplayMessage = "Unlock Door"; // Message displayed when the door can be unlocked
    public string cantBeOpenedDisplayMessage = "The door cannot be opened"; // Message displayed when the door can't be opened
    
    //Sound variables
    public AudioSource openSound; // Sound to play when the door opens
    public AudioSource closeSound; // Sound to play when the door closes
    public AudioSource lockedSound; // Sound to play when the door is locked
    public AudioSource unlockSound; // Sound to play when the door is unlocked
    public AudioSource knockSound; // Sound to play when the door is knocked

    //Shaking door variables
    public float shakeAmount = 0.05f; // Amount of shake
    public float shakeDuration = 1f; // Duration of shake
    private Vector3 originalPosition; // Store the original position of the door for shaking effect

    public void Awake()
    {
        interactable = GetComponent<Interactable>(); // Get the interactable component
        originalPosition = transform.position; // Store the original position for later shake effect
        if (isLocked) //If the door is locked
        {
            interactable.displayMessage = lockedDisplayMessage; // Set display message to lockedMessage
        }
        else
        {
            interactable.displayMessage = openDisplayMessage; // Set display message to openMessage
        }
    }

    public void Open() //Opens the door, can only be called, if player has the key!
    {
        openSound.Play(); // Play the open sound when opening the door
        StartCoroutine(RotateDoor(openAngle)); // Start rotating the door to open position
        interactable.displayMessage = closeDisplayMessage; // Update the display message to close when door is open
    } 

    public void Close() //Closes the door
    {
        closeSound.Play();
        StartCoroutine(RotateDoor(closeAngle)); // Start rotating the door to close position
        interactable.displayMessage = openDisplayMessage; // Update the display message to openMEssage when door is closed
    } 

    public void Unlock() //Unlocks the door
    {
        unlockSound.Play(); // Play the unlock sound when unlocking the door
        interactable.displayMessage = openDisplayMessage; // Update the display message to openMessage when door is unlocked
        isLocked = false; // Set the door to unlocked
    }

    public void InteractWithDoor() //Interacts with the door, called in the interactEent on the interactable script in the inspector.
    {
        if (cantBeOpened) return; // If the door cannot be opened, then return
        if(!isLocked || isAnimating) //If door is not locked, then open or close the door
        {
            if (isOpen) //If open
            {
                Close(); //close the door
            }
            else //if closed
            {
                Open(); //open the door
            }
            isOpen = !isOpen; // Toggle the door's open state
        }
        else if (isLocked && hasKey) //If door is locked and the player has the key, then Unlock the door
        {
            Unlock();
        }
        else if (shakeDoorOnly) //If door has been forced shut and can only be shaken
        {
            StartCoroutine(ShakeDoor()); // Start the door shaking coroutine
        }
        else //If door is locked and player does not have the key
        {
            interactable.displayMessage = lockedDisplayMessage; // Show locked message
            StartCoroutine(ShakeDoor()); // Shake the door
        }
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        isAnimating = true; // Prevent other animations while the door is rotating
        float currentAngle = transform.localRotation.eulerAngles.y; // Get the current angle of the door
    
        float time = 0f; // Timer for rotation interpolation

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f) // While the door hasn't reached the target angle
        {
            time += Time.deltaTime * openSpeed; // Update the time based on frame rate and speed
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, time); // Smoothly rotate the door using linear interpolation
            transform.localRotation = Quaternion.Euler(xAngle, currentAngle, 0f); // Apply the rotation to the door
            yield return null; // Wait for the next frame
        }

        transform.localRotation = Quaternion.Euler(xAngle, targetAngle, 0f); // Snap to final angle
        isAnimating = false; // Allow animations again
    }

    private IEnumerator ShakeDoor()
    {
        float elapsedTime = 0f; // Timer for the shaking duration
        lockedSound.Play(); // Play the locked sound when shaking

        while (elapsedTime < shakeDuration) // While the shake duration is not over
        {
            float shakeZ = Random.Range(-shakeAmount, shakeAmount); // Randomize the shake amount

            transform.position = originalPosition + new Vector3(0f, 0f, shakeZ); // Apply the shake effect

            elapsedTime += Time.deltaTime; // Increase elapsed time
            yield return null; // Wait for the next frame
        }

        transform.position = originalPosition; // Reset the position of the door after shaking
    }

    public void SetUnlockDisplayMessage() //Called when the player picks up the key
    {
        interactable.displayMessage = unlockDisplayMessage; // Set the display message to unlock the door
    }

    public void KnockDoor() //Called when the player interact with triggerCollider in hallway
    {
        StartCoroutine(KnockDoorRoutine()); // Start the knock door coroutine
    }

    private IEnumerator KnockDoorRoutine()
    {
        float knockSpeed = 0.1f; // Speed of the knock movement
        float knockDistance = 0.2f; // How far the door moves for each knock
        int knockCount = 5; // Number of knocks

        int waitTime = 2; // Time to wait before starting the knock
        yield return new WaitForSeconds(waitTime); //Waits for 2 seconds before starting the knock
        Vector3 originalPosition = transform.position; // Save the original position
        knockSound.volume = 0.5f; // Set the volume to the louder value (maximum, or you can choose any value)
        knockSound.Play(); // Play the knock sound

        for (int i = 0; i < knockCount; i++)
        {
            transform.position = originalPosition + transform.forward * knockDistance; // Move the door slightly forward
            yield return new WaitForSeconds(knockSpeed); // Wait for the knock duration

            transform.position = originalPosition; // Move the door back to its original position
            yield return new WaitForSeconds(knockSpeed); // Wait for the knock duration
        }
        transform.position = originalPosition; // Ensure the door ends in its original position
        
        // Do it all over again
        yield return new WaitForSeconds(waitTime);
        knockSound.volume = 1.0f; // Set the volume to make the knock sound louder for the second round
        knockSound.Play(); // Play the knock sound
        knockDistance = 0.4f; // Increase the knock distance for the second round

        for (int i = 0; i < knockCount; i++)
        {
            transform.position = originalPosition + transform.forward * knockDistance; // Move the door slightly forward
            yield return new WaitForSeconds(knockSpeed); // Wait for the knock duration

            transform.position = originalPosition; // Move the door back to its original position
            yield return new WaitForSeconds(knockSpeed); // Wait for the knock duration
        }
        transform.position = originalPosition; // Ensure the door ends in its original position
        knockSound.volume = 0.5f; // Reset the knock sound volume to original
    }

    public void LockDoor() //Called when player enters triggerCollider in DiningRoom
    {
        isLocked = true; //Set bool to true
        hasKey = false; // Ensure the player no longer has the key
        if (isOpen)
        {
            Close(); // Close the door if it is currently open
        }
        interactable.displayMessage = cantBeOpenedDisplayMessage; // Set display message to indicate the door can't be opened
        shakeDoorOnly = true; // Enable the shaking effect for the locked door
    }

    public void BlockDoor() //Called when layer enters triggerCollider in hallway, so it blocks the door blocked by the cabinet
    {
        cantBeOpened = true; // Set the door to be unopenable
        interactable.displayMessage = cantBeOpenedDisplayMessage; // Set the display message to indicate the door can't be opened
        isLocked = true; // Set the door to be locked
        hasKey = false; // Ensure the player no longer has the key

    }

    public void UnblockDoor() //Called when the player moves the cabinet
    {
        cantBeOpened = false; // Set the door to be openable
        interactable.displayMessage = openDisplayMessage; // Set the display message to indicate the door can be opened
        isLocked = false;  // Set the door to be unlocked
        hasKey = true; // Ensures the player can open the door without having picked up a key
    }
}

