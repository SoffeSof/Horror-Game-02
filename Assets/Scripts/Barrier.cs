using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Barrier : MonoBehaviour
{
    public bool hasMagicBall = false;
    public UnityEvent interactEvent;

    
    public void AddMagicBall()
    {
        hasMagicBall = true;
        CheckForMagicBallAndNote();
    }

    public void CheckForMagicBallAndNote()
    {
        if (hasMagicBall && HUDController.Instance.allNotesCollected == true)
        {
            HUDController.Instance.RunText();
            MusicManager.Instance.ChangeMusic();
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interactEvent.Invoke(); //Calls the event set in the inspector
        }
    }
}
