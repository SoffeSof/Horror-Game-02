using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    
    public float pushSpeed = 2f; // Speed at which the cabinet moves
    private float endPosition = -14.59f; // Ending Z position of the cabinet

    private bool isMovingForward = true; // Direction of the movement
    private bool hasBeenPushed = false; //Bool to prevent multiple pushes

    private Interactable interactable; // Reference to the interactable component
    private Outline outline; // Reference to the outline component

    void Start()
    {
        interactable = GetComponent<Interactable>(); // Get the interactable component
        outline = GetComponent<Outline>(); // Get the outline component
        interactable.displayMessage = "Push Cabinet"; // Set the displayeMessage in the interactable script to "Push Cabinet"
    }

    public void PushCabinet() //Method called when interacting (pressing E) the the cabinet, called in the iteractEvent in the Interactable script set in the inspector
    {
        if (!isMovingForward || hasBeenPushed == true) return; // Prevent multiple pushes at the same time
        outline.enabled = false; // Disable the outline when the cabinet is pushed
        interactable.enabled = false; // Disable the interactable component to stop further interactions
        StartCoroutine(MoveCabinet()); // Start the cabinet movement coroutine
        hasBeenPushed = true; // Mark the cabinet as already pushed
    }

    private IEnumerator MoveCabinet()
    {
        isMovingForward = true; // Mark as moving
        Vector3 startPosition = transform.position; // Starting position as a Vector3
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, endPosition); // Target position

        float time = 0f; // Tracks interpolation progress

        while (Mathf.Abs(transform.position.z - endPosition) > 0.1f) // While the cabinet hasn't reached the target position
        {
            time += Time.deltaTime * pushSpeed; // Increment the time based on frame time and push speed
            transform.position = Vector3.Lerp(startPosition, targetPosition, time); // Smoothly move the cabinet to the target position
            yield return null; // Wait until the next frame to continue
        }
        transform.position = targetPosition; // Snap to the final position for precision
        isMovingForward = false; //Mark as not moving
    }
}

