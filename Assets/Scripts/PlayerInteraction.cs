using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float playerReach = 3f;
    private Interactable currentInteractable;

    // Update is called once per frame
    void Update()
    {
        CheckForInteractable();
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            Debug.Log("E pressed");
            currentInteractable.Interact();
        }
    }

    public void CheckForInteractable()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); //shoots a ray straight from the center of camera

        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();
                //if there is a currentIteractable and it is not the same as the newInteractable
                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }

                if (newInteractable.enabled)
                {
                    SetCurrentInteractable(newInteractable);
                }
                else //if the item is not enabled
                {
                    DisableCurrentInteractable();
                }
            }
            else //if the item is not interactable
            {
                DisableCurrentInteractable();
            }
            
        }
        else //if the raycast doesn't hit anything / nothing is within reach
        {
            DisableCurrentInteractable();
        }
        
            
    }

    public void SetCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        HUDController.Instance.EnableInteractionText(currentInteractable.displayMessage, currentInteractable.itemType);
    }

    public void DisableCurrentInteractable()
    {
        HUDController.Instance.DisableInteractableText();
        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
