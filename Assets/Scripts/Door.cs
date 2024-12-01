using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Scripts
    public Interactable interactable;

    //Door State Variables
    public bool isOpen = false;
    public bool isLocked = false;
    private bool isAnimating = false;
    public bool hasKey = false;
    public bool cantBeOpened = false;

    //Door Movement Variables
    public float openSpeed = 0.5f;
    public float closeSpeed = 1.0f;
    public float openAngle = 90.0f;
    public float closeAngle = 0.0f;
    public float xAngle = 0.0f;

    //Display messages
    public string openDisplayMessage = "Open Door";
    public string closeDisplayMessage = "Close Door";
    public string lockedDisplayMessage = "Locked";
    public string unlockDisplayMessage = "Unlock Door";

    //Sound variables
    public AudioSource openSound; // Sound to play when the door opens
    public AudioSource closeSound; // Sound to play when the door closes
    public AudioSource lockedSound; // Sound to play when the door closes
    public AudioSource unlockSound; // Sound to play when the unlocks
    public AudioSource knockSound; // Sound to play when the door is knocked

    //Shaking door variables
    public float shakeAmount = 0.05f; // Amount of shake
    public float shakeDuration = 1f; // Duration of shake

    private Vector3 originalPosition;

    public void Awake()
    {
        interactable = GetComponent<Interactable>();
        originalPosition = transform.position; // Store the original position
        if (isLocked)
        {
            interactable.displayMessage = lockedDisplayMessage;
        }
        else
        {
            interactable.displayMessage = openDisplayMessage;
        }
    }

    public void Open()
    {
        openSound.Play();
        StartCoroutine(RotateDoor(openAngle));
        interactable.displayMessage = closeDisplayMessage;
    }

    public void Close()
    {
        closeSound.Play();
        StartCoroutine(RotateDoor(closeAngle));
        interactable.displayMessage = openDisplayMessage;
    }

    public void Unlock() //Use enumerator to play the sound when unlocking the door
    {
        unlockSound.Play();
        interactable.displayMessage = openDisplayMessage;
        isLocked = false;
    }

    public void InteractWithDoor()
    {
        if (cantBeOpened) return;
        if(!isLocked || isAnimating) //If door is not locked, then open or close the door
        {
            if (isOpen) 
            {
                Close();
            }
            else
            {
                Open();
            }
            isOpen = !isOpen;
        }
        else if (isLocked && hasKey) //If door is locked and has key, then Unlock the door and open it
        {
            Unlock();
        }
        else
        {
            interactable.displayMessage = lockedDisplayMessage;
            StartCoroutine(ShakeDoor());
        }
    }

    private IEnumerator RotateDoor(float targetAngle)
    {
        isAnimating = true;
        float currentAngle = transform.localRotation.eulerAngles.y;
    
        float time = 0f;

        while (Mathf.Abs(currentAngle - targetAngle) > 0.1f)
        {
            time += Time.deltaTime * openSpeed;
            currentAngle = Mathf.Lerp(currentAngle, targetAngle, time);
            transform.localRotation = Quaternion.Euler(xAngle, currentAngle, 0f);
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(xAngle, targetAngle, 0f); // Snap to final angle
        isAnimating = false;
    }

    private IEnumerator ShakeDoor()
    {
        float elapsedTime = 0f;
        lockedSound.Play();

        while (elapsedTime < shakeDuration)
        {
            float shakeZ = Random.Range(-shakeAmount, shakeAmount);

            transform.position = originalPosition + new Vector3(0f, 0f, shakeZ);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // Reset position after shaking
    }

    public void SetUnlockDisplayMessage()
    {
        interactable.displayMessage = unlockDisplayMessage;
    }

    public void KnockDoor()
    {
        StartCoroutine(KnockDoorRoutine());
    }

    private IEnumerator KnockDoorRoutine()
    {
        float knockSpeed = 0.1f; // Speed of the knock movement
        float knockDistance = 0.2f; // How far the door moves for each knock
        int knockCount = 5; // Number of knocks

        int waitTime = 2; // Time to wait before starting the knock
        yield return new WaitForSeconds(waitTime);
        Vector3 originalPosition = transform.position; // Save the original position
        knockSound.volume = 0.5f; // Set the volume to the louder value (maximum, or you can choose any value)
        knockSound.Play(); // Play the knock sound

        for (int i = 0; i < knockCount; i++)
        {
            // Move the door slightly forward
            transform.position = originalPosition + transform.forward * knockDistance;
            //knockSound.Play(); // Play the knock sound
            yield return new WaitForSeconds(knockSpeed);

            // Move the door back to its original position
            transform.position = originalPosition;
            yield return new WaitForSeconds(knockSpeed);
        }

        // Ensure the door ends in its original position
        transform.position = originalPosition;

        yield return new WaitForSeconds(waitTime);
        // Make the knock sound louder for the second round
        knockSound.volume = 1.0f; // Set the volume to the louder value (maximum, or you can choose any value)
        knockSound.Play(); // Play the knock sound
        knockDistance = 0.4f; // How far the door moves for each knock

        for (int i = 0; i < knockCount; i++)
        {
            // Move the door slightly forward
            transform.position = originalPosition + transform.forward * knockDistance;
            //knockSound.Play(); // Play the knock sound
            yield return new WaitForSeconds(knockSpeed);

            // Move the door back to its original position
            transform.position = originalPosition;
            yield return new WaitForSeconds(knockSpeed);
        }
         // Ensure the door ends in its original position
        transform.position = originalPosition;
        knockSound.volume = 0.5f; // Set the volume to the louder value (maximum, or you can choose any value)
    }

    public void LockDoor()
    {
        isLocked = true;
        hasKey = false;
        cantBeOpened = true;
        if (isOpen)
        {
            Close();
            isOpen = false;
            interactable.displayMessage = "The door cannot be opened";
        }
    }
}

