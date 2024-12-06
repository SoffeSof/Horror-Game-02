using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 5f; // Distance the player can interact with objects
    private Interactable currentInteractable; // Holds the currently interactable object

    // Update is called once per frame
    void Update()
    {
        CheckForInteractable(); // Check if the player is looking at an interactable object
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null) // If the player presses 'E' and there's an interactable object, interact with it
        {
            currentInteractable.Interact(); // Perform interaction with the current object
        }
    }

    public void CheckForInteractable() // Checks for interactable objects within reach
    {
        RaycastHit hit; // Stores information about the object hit by the ray
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); //shoots a ray straight from the center of camera

        if (Physics.Raycast(ray, out hit, playerReach)) // Check if the ray hits an object within the player's reach
        {
            if (hit.collider.tag == "Interactable") // If the hit object is tagged as "Interactable"
            { 
                Interactable newInteractable = hit.collider.GetComponent<Interactable>(); // Get the Interactable component attached to the object
                if (currentInteractable && newInteractable != currentInteractable)  // If there's already a currentInteractable and it's different from the new one
                {
                    currentInteractable.DisableOutline(); //Disable outline on the old one
                }

                if (newInteractable.enabled) // If the new interactable is enabled
                {
                    SetCurrentInteractable(newInteractable); //set it as the current interactable and enable its outline
                }
                else //if the item is not enabled
                {
                    DisableCurrentInteractable(); //disable the current interactable
                }
            }
            else //if the item is not interactable
            {
                DisableCurrentInteractable();
            }
            
        }
        else //if the raycast doesn't hit anything / nothing is within reach
        {
            DisableCurrentInteractable(); //disable the current interactable
        }
        
            
    }

    public void SetCurrentInteractable(Interactable newInteractable) // Sets the current interactable object
    {
        currentInteractable = newInteractable; // Set the new interactable as the current one
        currentInteractable.EnableOutline(); // Enable the outline of the new interactable
        HUDController.Instance.EnableInteractionText(currentInteractable.displayMessage, currentInteractable.itemType); // Display the interaction text on the HUD
    }

    public void DisableCurrentInteractable() // Disables the current interactable object
    {
        HUDController.Instance.DisableInteractableText(); // Disable the interaction text on the HUD
        if (currentInteractable != null) // If there's a current interactable object
        {
            currentInteractable.DisableOutline(); // Disable the outline of the current interactable
            currentInteractable = null; // Set the current interactable to null
        }
    }
}
