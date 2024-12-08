using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollider : MonoBehaviour
{
    public Door door; // Reference to the Door object that will be interacted with
    public UnityEvent interactEvent;  // UnityEvent that can be set in the Inspector to invoke when the trigger occurs
    public GameObject monster; // Reference to the monster object that will be interacted with

    public void OnTriggerEnter(Collider other) // This method is called when another collider enters the trigger collider
    {
        if (other.tag == "Player") // Check if the object that triggered the collider is tagged as "Player"
        {
            interactEvent.Invoke(); //Calls the event set in the inspector
            Destroy(gameObject);  // Destroys the object this script is attached to
        }
    }
    public void GameDone() //Method called on the ending door in the forest level
    {
        HUDController.Instance.GameDone(); // Calls the GameDone method in the HUDController
        monster.SetActive(false); // Deactivates the monster object
    }
}
