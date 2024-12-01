using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCollider : MonoBehaviour
{
    public Door door;
    public UnityEvent interactEvent;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactEvent.Invoke(); //Calls the event set in the inspector
        }
    }

    public void KnockDoor()
    {
        door.KnockDoor();
        Destroy(gameObject);
    }

    public void LockDoor()
    {
        door.LockDoor();
        Destroy(gameObject);
    }
}
