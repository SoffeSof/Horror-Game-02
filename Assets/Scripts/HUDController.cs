using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    // Static instance that can be accessed from other scripts / Singleton
    public static HUDController Instance { get; private set; }

    private void Awake()
    {
        // Check if the instance already exists and destroy this one if it does
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Set the instance to this and make it persist across scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private string textInFront = "[E] ";

    public void EnableInteractionText(string text)
    {
        interactionText.text = textInFront + text;
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractableText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
