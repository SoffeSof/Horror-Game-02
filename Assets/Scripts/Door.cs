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

    //Door Movement Variables
    public float openSpeed = 0.5f;
    public float closeSpeed = 1.0f;
    public float openAngle = 90.0f;
    public float closeAngle = 0.0f;

    private float currentAngle = 0.0f;

    //Display messages
    public string openDisplayMessage = "Open Door";
    public string closeDisplayMessage = "Close Door";
    public string lockedDisplayMessage = "Locked";

    //Sound variables
    public AudioSource openSound; // Sound to play when the door opens
    public AudioSource closeSound; // Sound to play when the door closes
    public AudioSource lockedSound; // Sound to play when the door closes

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
    }

    public void Close()
    {
        closeSound.Play();
        StartCoroutine(RotateDoor(closeAngle));
    }

    public void InteractWithDoor()
    {
        if(!isLocked || isAnimating) //If door is not locked, then open or close the door
        {
            if (isOpen) 
            {
                Close();
                interactable.displayMessage = openDisplayMessage;
            }
            else
            {
                Open();
                interactable.displayMessage = closeDisplayMessage;
            }
            isOpen = !isOpen;
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
            transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0f, targetAngle, 0f); // Snap to final angle
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

    //Make a method to unlock door be checking the inventiry for the correct key.
}

