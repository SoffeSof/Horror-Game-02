using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBase : MonoBehaviour
{
    public bool hasLever = false;
    public bool pullLever = false;
    private bool hasBeenOpened = false;
    private string leverMissingText = "The lever is missing";
    private string insertleverText = "Insert lever";
    private string pullLeverText = "Pull lever";    
    private Interactable interactable;
    public InventoryManager inventoryManager;
    public GameObject lever;
    public GameObject door;
    public Item leverItem;
    public AudioSource openSound;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        interactable.displayMessage = leverMissingText;
    }

    public void InsertLever()
    {
        if (hasBeenOpened)
        {
            return;
        }
        if (pullLever)
        {
            PullLeverOpenDoor();
            hasBeenOpened = true;
        }
        if (hasLever)
        {
            lever.SetActive(true);
            inventoryManager.RemoveItem(leverItem);
            interactable.displayMessage = pullLeverText;
            pullLever = true;
        }
    }

    public void AddLever()
    {
        hasLever = true;
        interactable.displayMessage = insertleverText;
    }

    public void PullLeverOpenDoor()
    {
        StartCoroutine(RotateLeverOpenDoor());
    }
    
    private IEnumerator RotateLeverOpenDoor()
    {
        float duration = 5f; // Time it takes to rotate (in seconds)
        float elapsedTime = 0f;

        // Start and target rotations for the lever
        Quaternion startRotation = lever.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(45, 0, 0); // Target rotation

        // Start and target positions for the door
        Vector3 startDoorPosition = door.transform.position;
        Vector3 endDoorPosition = startDoorPosition + new Vector3(0, 5, 0); // Move door up by 5 units
        openSound.Play();
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Rotate the lever
            lever.transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            // Move the door
            door.transform.position = Vector3.Lerp(startDoorPosition, endDoorPosition, elapsedTime / duration);
            yield return null;
        }

        // Ensure the lever ends exactly at the target rotation
        lever.transform.rotation = endRotation;
        door.transform.position = endDoorPosition;
    }

    public void OpenDoor()
    {
        //needs to open the door
    }
}
