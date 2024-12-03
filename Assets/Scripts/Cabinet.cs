using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    
    public float pushSpeed = 2f; // Speed at which the cabinet moves
    private float endPosition = -14.59f; // Ending Z position of the cabinet

    private bool isMovingForward = true; // Direction of the movement
    private bool hasBeenPushed = false; // Prevents multiple pushes

    private Interactable interactable;
    private Outline outline;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        outline = GetComponent<Outline>();
        interactable.displayMessage = "Push Cabinet";
    }

    public void PushCabinet()
    {
        if (!isMovingForward || hasBeenPushed == true) return; // Prevent multiple pushes at the same time
        outline.enabled = false;
        interactable.enabled = false;
        StartCoroutine(MoveCabinet());
        hasBeenPushed = true;
    }

    private IEnumerator MoveCabinet()
    {
        isMovingForward = true; // Mark as moving
        Vector3 startPosition = transform.position; // Starting position as a Vector3
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, endPosition); // Target position

        float time = 0f; // Tracks interpolation progress

        while (Mathf.Abs(transform.position.z - endPosition) > 0.1f)
        {
            time += Time.deltaTime * pushSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, time); // Smoothly move towards the target
            yield return null;
        }

        // Snap to the final position for precision
        transform.position = targetPosition;
        isMovingForward = false; // Mark as not moving
    }
}

