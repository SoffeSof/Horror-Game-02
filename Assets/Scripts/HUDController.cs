using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    // Static instance that can be accessed from other scripts / Singleton
    public static HUDController Instance { get; private set; }
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private NoteInventoryManager noteInventoryManager;
    [SerializeField] private Flashlight flashlight;

    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private string textInFront = "[E] ";
    [SerializeField] private TMP_Text notesText;
    [SerializeField] private TMP_Text batteryText;

    //Healthbar variables
    //HealthbarTutorial followed: https://www.youtube.com/watch?v=BLfNP4Sc_iA&t=50s by Brackeys
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private float healthDrainRate = 100f/500f;

    //Get set for health
    [SerializeField] private float _health = 100f; // Private backing field

    public float Health //Single source of truth principle
    {
        get { return _health; }
        set {
                _health = Mathf.Clamp(value, 0f, 100f); // Clamps the value between 0 and 100
                UpdateHealthBar();

                if (_health <= 0)
                {
                    GameOver();
                }
            } 
    }

    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        noteInventoryManager = FindObjectOfType<NoteInventoryManager>();
        flashlight = FindObjectOfType<Flashlight>();

        // Healtbar 
        Health = 10f; // Set the health to the initial value
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

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

    void Update()
    {
        DrainHealth();
    }

    public void EnableInteractionText(string text, Item item)
    {
        bool isInventoryFull = inventoryManager.CheckInventorySpace(item);
        if (isInventoryFull)
        {
            interactionText.text = "Inventory is full";
        }
        else 
        {
            interactionText.text = textInFront + text;
        }
        interactionText.gameObject.SetActive(true);

    }

    public void DisableInteractableText()
    {
        interactionText.gameObject.SetActive(false);
    }

    public void UpdateNotesCollected()
    {
        notesText.text = $"{noteInventoryManager.NotesCollected} / 6 Notes Collected";

        int maxNotes = 6;
        if (noteInventoryManager.NotesCollected == maxNotes)
        {
            notesText.text = "All notes collected! FIND THE WAY OUT!";
        }
    }

    public void UpdateHealthBar()
    {
        slider.value = Health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void GameOver()
    {
        Debug.Log("Player is dead");
    }

    public void UpdateBatteryText()
    {
        batteryText.text = $"Battery: {flashlight.Energy:F0}%"; // F0 formats it as a whole number (no decimal places)
    }

    private void DrainHealth()
    {
        if (flashlight.isOn)
        {
            Health -= healthDrainRate * Time.deltaTime;
        }
    }
}
