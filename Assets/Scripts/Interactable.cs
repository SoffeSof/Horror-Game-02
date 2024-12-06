using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public Outline outline; //Outline component for the interactable object
    public string displayMessage; //Message to display when the player is near the object

    public UnityEvent interactEvent; //Event to call when the player interacts with the object
    public Item itemType; //Item type of the object collected, if going to the inventory


    // Start is called before the first frame update
    void Start()
    {
        DisableOutline(); //Disables the outline at the start
    }

    public void Interact() //Function to call when the player interacts with the object
    {
        interactEvent.Invoke(); //Calls the event set in the inspector
    }

    public void DisableOutline() //Function to disable the outline
    {
        outline.enabled = false; //Disables the outline
    }

    public void EnableOutline() //Function to enable the outline
    {
        outline.enabled = true; //Enables the outline
    }
}
