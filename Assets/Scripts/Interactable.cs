using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public Outline outline;
    public string displayMessage;

    public UnityEvent interactEvent;


    // Start is called before the first frame update
    void Start()
    {
        DisableOutline();
    }

    public void Interact()
    {
        interactEvent.Invoke(); //Calls the event set in the inspector
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        outline.enabled = true;
    }

    
}
