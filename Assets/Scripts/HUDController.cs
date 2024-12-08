using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; } // Static instance that can be accessed from other scripts / Singleton

    // References to other game components
    [SerializeField] private InventoryManager inventoryManager;  // Manages the inventory
    [SerializeField] private NoteInventoryManager noteInventoryManager;  // Manages notes in the inventory
    [SerializeField] private KeyInventoryManager keyInventoryManager;  // Manages keys in the inventory
    [SerializeField] private Flashlight flashlight;  // Manages the flashlight
    [SerializeField] private Barrier barrier;  // Represents the barrier object
    [SerializeField] private PlayerMovement playerMovement;  // Controls player movement

    public bool allNotesCollected = false;  // Keeps track of whether all notes are collected

    // UI Text elements for interaction prompts and status messages
     [SerializeField] private TMP_Text interactionText;  // Text shown for player interactions
    [SerializeField] private TMP_Text screenText;  // General screen text for displaying messages
    [SerializeField] private string textInFront = "[E] ";  // Prefix for interaction text
    [SerializeField] private TMP_Text notesText;  // Text showing the number of notes collected
    [SerializeField] private TMP_Text batteryText;  // Text showing battery percentage of the flashlight
    [SerializeField] private GameObject gameDonePanel;  // UI panel for when the game is done
    [SerializeField] private GameObject gameOverPanel;  // UI panel for when the game is over
    [SerializeField] private TMP_Text sprintText;  // Text showing sprint energy

    // Audio components for the heartbeat and whispering sounds
    [SerializeField] private AudioSource heartbeatAudioSource;  // Audio source for heartbeat sound
    [SerializeField] private AudioSource whisperingAudioSource;  // Audio source for whispering sound
    [SerializeField] private AudioClip heartbeatClip;  // Heartbeat audio clip
    [SerializeField] private AudioClip whisperingClip;  // Whispering audio clip

    // Other audio components
    [SerializeField] private AudioSource creepyJumpScareSound;  // Audio source for the jump scare sound


    //Healthbar variables
    public Slider slider;  // UI slider representing the sanity level
    public Gradient gradient;  // Gradient for health bar color changes
    public Image fill;  // Image that fills the health bar
    public float sanityDrainRate = 0.5f;  // Rate at which sanity decreases

    //Get set for health
    [SerializeField] private float _sanity = 100f; // Private backing field

    public float Sanity // Public property for sanity with getters and setters Single source of truth principle
    {
        get { return _sanity; } // Returns the current sanity value
        set {
                _sanity = Mathf.Clamp(value, 0f, 100f); // Clamps the value between 0 and 100
                UpdateSanityBar(); // Updates the health bar when sanity changes

                if (_sanity <= 0) // Checks if sanity is 0 or below, triggering a game over
                {
                    GameOver();
                }
                else
                {
                    UpdateAudioEffects(); // Adjusts the audio effects based on sanity level
                }
            } 
    }

    void Start()
    {
        // Initial setup for the health bar and audio components
        fill.color = gradient.Evaluate(slider.normalizedValue);  // Sets the health bar color based on the initial value

        // Initialize audio sources
        heartbeatAudioSource.clip = heartbeatClip;
        whisperingAudioSource.clip = whisperingClip;

        // Set audio sources to loop
        heartbeatAudioSource.loop = true;
        whisperingAudioSource.loop = true;

        // Start playing heartbeat and whispering sounds
        heartbeatAudioSource.Play();
        whisperingAudioSource.Play();
    }

    private void Awake()
    {
        // Ensures only one instance of HUDController exists (Singleton pattern)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroys any extra instances
        }
        else
        {
            // Set the instance to this and make it persist across scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Finds and assigns references to other game components
        playerMovement = FindObjectOfType<PlayerMovement>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        noteInventoryManager = FindObjectOfType<NoteInventoryManager>();
        keyInventoryManager = FindObjectOfType<KeyInventoryManager>();
        flashlight = FindObjectOfType<Flashlight>();
        barrier = GameObject.Find("Barrier").GetComponent<Barrier>();
    }

    public void GetReferences()
    {
        // Re-initializes references to the game components, called when loading a new scene
        playerMovement = FindObjectOfType<PlayerMovement>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        noteInventoryManager = FindObjectOfType<NoteInventoryManager>();
        keyInventoryManager = FindObjectOfType<KeyInventoryManager>();
        flashlight = FindObjectOfType<Flashlight>();
    }

    private void UpdateAudioEffects()  // Adjusts audio effects based on sanity level
    {
        if (Sanity < 50f) // Enable heartbeat and whispering only when sanity is below 50
        {
            if (!heartbeatAudioSource.isPlaying) // If the heartbeat sound is not playing
            {
                heartbeatAudioSource.Play(); // Start playing the heartbeat sound
            }
            float heartbeatIntensity = Mathf.InverseLerp(50f, 0f, Sanity); // Normalized between 0 and 1
            heartbeatAudioSource.volume = Mathf.Lerp(0f, 0.6f, heartbeatIntensity); // Volume increases with lower sanity
            heartbeatAudioSource.pitch = Mathf.Lerp(1f, 1.8f, heartbeatIntensity); // Pitch increases with lower sanity

            // Activate whispering sound
            if (!whisperingAudioSource.isPlaying) // If the whispering sound is not playing
            {
                whisperingAudioSource.Play(); // Start playing the whispering sound
            }
            float whisperingIntensity = Mathf.InverseLerp(50f, 0f, Sanity); // Normalized between 0 and 1
            whisperingAudioSource.volume = Mathf.Lerp(0f, 0.4f, whisperingIntensity); // Volume increases with lower sanity
        }
        else  // Stops both heartbeat and whispering sounds when sanity is above 50
        {
            // Deactivate sounds when sanity is above 50
            if (heartbeatAudioSource.isPlaying)
            {
                heartbeatAudioSource.Stop();
            }

            if (whisperingAudioSource.isPlaying)
            {
                whisperingAudioSource.Stop();
            }
        }
    }

    void Update()
    {
        DrainSanity(); // Drains sanity over time
    }

    public void EnableInteractionText(string text, Item item) // Displays interaction text
    {
        bool isInventoryFull = inventoryManager.CheckInventorySpace(item);  // Checks if the inventory has space for the item
        if (isInventoryFull)
        {
            interactionText.text = "Inventory is full"; // Displays inventory full message
        }
        else 
        {
            interactionText.text = textInFront + text; // Displays the given interaction text
        }
        interactionText.gameObject.SetActive(true); // Shows the interaction text

    }

    public void EnableCantBeUsedInteractionText(string text)
    {
        StartCoroutine(CantBeUsedInteractionText(text)); // Starts a coroutine to display a message that an item can't be used
    }

    private IEnumerator CantBeUsedInteractionText(string text)
    {
        // Displays the "can't be used" message for a short time
        screenText.text = text; // Sets the text 
        screenText.gameObject.SetActive(true); // Shows the text
        yield return new WaitForSeconds(5f); // Wait for the specified time
        screenText.gameObject.SetActive(false); // Disable the text
    }

    public void DisableInteractableText()
    {
        interactionText.gameObject.SetActive(false); // Disables the interaction text
    }

    public void UpdateNotesCollected() //Gets called when a note is collected
    {
        notesText.text = $"{noteInventoryManager.NotesCollected} / 6 Notes Collected"; // Updates the notes collected text

        int maxNotes = 6; //max number of notes
        if (noteInventoryManager.NotesCollected == maxNotes) // Checks if all notes are collected
        {
            notesText.text = "All notes collected!"; // Displays a message when all notes are collected
            allNotesCollected = true; // Marks that all notes are collected
        }
    }

    public void UpdateSanityBar()  // Updates the health bar UI based on the current sanity
    {
        slider.value = Sanity; // Sets the slider value to the current sanity level
        fill.color = gradient.Evaluate(slider.normalizedValue); // Updates the color of the health bar
    }

    public void GameOver() // Displays the game over panel when the game ends
    {
        gameOverPanel.SetActive(true); // Displays the game over panel
    }

    public void UpdateBatteryText() // Updates the flashlight's battery UI text
    {
        batteryText.text = $"Battery: {flashlight.Energy:F0}%"; //Displays the battery percentage. F0 formats it as a whole number (no decimal places)
    }

    public void UpdateSprintText() // Updates the sprint energy UI text
    {
        sprintText.text = $"Sprint Energy: {playerMovement.SprintEnergy:F0}%"; // Displays the sprint energy. F0 formats it as a whole number (no decimal places)
    }

    private void DrainSanity() // Drains sanity over time
    {
        Sanity -= sanityDrainRate * Time.deltaTime; // Decreases sanity over time
    }

    public void CantLeaveText() // Displays a message that the player can't leave yet, called when colliding with the barrier
    { 
        StartCoroutine(CantLeaveEnumerator()); // Starts a coroutine to display the message
    }

    private IEnumerator CantLeaveEnumerator() // Displays a message that the player can't leave yet
    {
        screenText.gameObject.SetActive(true);  // Enable the text
        if (barrier.hasMagicBall == false && allNotesCollected == false) // Checks if the magic item and all notes are missing
        {
            screenText.text = $"You can't leave yet! The Magic item and {6 - noteInventoryManager.NotesCollected} notes are missing"; // Displays the message
        }
        else if (barrier.hasMagicBall == false) // Checks if the magic item is missing
        {
            screenText.text = "You can't leave yet! The magic item is missing!"; // Displays the message
        }
        else if (allNotesCollected == false) // Checks if all notes are missing
        {
            if (6 - noteInventoryManager.NotesCollected == 1) // Checks if only one note is missing
            {
                screenText.text = "You can't leave yet! A note is missing!"; // Displays the message
            }
            else  // Displays the number of notes missing
            {
                screenText.text = $"You can't leave yet! {6 - noteInventoryManager.NotesCollected} notes are missing!"; // Displays the message
            }
        }
        yield return new WaitForSeconds(5f); // Wait for the specified time
        screenText.gameObject.SetActive(false); // Disable the text
    }

    public void RunText() // Displays a message to run from the monster
    {
        StartCoroutine(RunTextEnumator()); // Starts a coroutine to display the message
    }

    private IEnumerator RunTextEnumator() // Displays a message to run from the monster
    {
        creepyJumpScareSound.Play(); // Plays the jump scare sound
        screenText.gameObject.SetActive(true); // Enable the text
        if (keyInventoryManager.keyCount == 6) // Checks if the player has all the keys
        {
            screenText.text = "IT'S COMING FOR YOU. RUN NOW! YOU HAVE THE KEY FOR THE FRONT DOOR"; // Displays the message
        }
        else
        {
            screenText.text = "IT'S COMING FOR YOU. RUN NOW! GRAB THE KEY FOR THE FRONT DOOR"; // Displays the message
        }
        yield return new WaitForSeconds(5f); // Wait for the specified time
        screenText.gameObject.SetActive(false); // Disable the text
    }

    public void GameDone() // Displays the game done panel when the game ends
    {
        gameDonePanel.SetActive(true); // Displays the game done panel
    }

    public void ChangeScene() // Changes the scene to the forest
    {
        SceneManager.LoadScene("Forest"); // Loads the forest scene
    }
}
