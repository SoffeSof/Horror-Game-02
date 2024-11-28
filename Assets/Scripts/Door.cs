using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Interactable interactable;
    public bool isOpen = false;
    private bool isLocked = false;
    private bool isAnimating = false;
    public float openSpeed = 0.5f;
    public float closeSpeed = 1.0f;
    public float openAngle = 90.0f;
    public float closeAngle = 0.0f;

    private float currentAngle = 0.0f;

    public string openDisplayMessage = "Open Door";
    public string closeDisplayMessage = "Close Door";

    public AudioSource openSound; // Sound to play when the door opens
    public AudioSource closeSound; // Sound to play when the door closes

    public void Awake()
    {
        interactable = GetComponent<Interactable>();
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
        }

        isOpen = !isOpen;
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
}

