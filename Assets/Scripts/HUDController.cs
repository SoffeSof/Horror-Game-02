using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management

public class HUDController : MonoBehaviour
{
    // Static instance that can be accessed from other scripts / Singleton
    public static HUDController Instance { get; private set; }
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private NoteInventoryManager noteInventoryManager;
    [SerializeField] private KeyInventoryManager keyInventoryManager;
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private Barrier barrier;
    [SerializeField] private PlayerMovement playerMovement;

    public bool allNotesCollected = false;


    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private TMP_Text screenText;
    [SerializeField] private string textInFront = "[E] ";
    [SerializeField] private TMP_Text notesText;
    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private GameObject gameDonePanel;
    [SerializeField] private TMP_Text sprintText;

     // Audio sanity variables
    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private AudioSource whisperingAudioSource;
    [SerializeField] private AudioClip heartbeatClip;
    [SerializeField] private AudioClip whisperingClip;

    //Other Audio
    [SerializeField] private AudioSource creepyJumpScareSound;


    //Healthbar variables
    //HealthbarTutorial followed: https://www.youtube.com/watch?v=BLfNP4Sc_iA&t=50s by Brackeys
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private float healthDrainRate = 100f/500f;

    //Get set for health
    [SerializeField] private float _sanity = 100f; // Private backing field

    public float Sanity //Single source of truth principle
    {
        get { return _sanity; }
        set {
                _sanity = Mathf.Clamp(value, 0f, 100f); // Clamps the value between 0 and 100
                UpdateHealthBar();

                if (_sanity <= 0)
                {
                    GameOver();
                }
                else
                {
                    UpdateAudioEffects();
                }
            } 
    }

    void Start()
    {
        // Healtbar 
        //Sanity = 10f; // Set the health to the initial value
        fill.color = gradient.Evaluate(slider.normalizedValue);

        // Initialize audio sources
        heartbeatAudioSource.clip = heartbeatClip;
        whisperingAudioSource.clip = whisperingClip;

        heartbeatAudioSource.loop = true;
        whisperingAudioSource.loop = true;

        heartbeatAudioSource.Play();
        whisperingAudioSource.Play();
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
        playerMovement = FindObjectOfType<PlayerMovement>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        noteInventoryManager = FindObjectOfType<NoteInventoryManager>();
        keyInventoryManager = FindObjectOfType<KeyInventoryManager>();
        flashlight = FindObjectOfType<Flashlight>();
        barrier = GameObject.Find("Barrier").GetComponent<Barrier>();
    }

    public void GetReferences()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        noteInventoryManager = FindObjectOfType<NoteInventoryManager>();
        keyInventoryManager = FindObjectOfType<KeyInventoryManager>();
        flashlight = FindObjectOfType<Flashlight>();
    }

    private void UpdateAudioEffects()
    {
            // Enable heartbeat and whispering only when sanity is below 50
        if (Sanity < 50f)
        {
            // Activate heartbeat sound
            if (!heartbeatAudioSource.isPlaying)
            {
                heartbeatAudioSource.Play();
            }
            float heartbeatIntensity = Mathf.InverseLerp(50f, 0f, Sanity); // Normalized between 0 and 1
            heartbeatAudioSource.volume = Mathf.Lerp(0f, 0.6f, heartbeatIntensity); // Max volume reduced to 0.6
            heartbeatAudioSource.pitch = Mathf.Lerp(1f, 1.8f, heartbeatIntensity); // Slightly reduced pitch range

            // Activate whispering sound
            if (!whisperingAudioSource.isPlaying)
            {
                whisperingAudioSource.Play();
            }
            float whisperingIntensity = Mathf.InverseLerp(50f, 0f, Sanity);
            whisperingAudioSource.volume = Mathf.Lerp(0f, 0.4f, whisperingIntensity); // Max volume reduced to 0.4
        }
        else
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

    public void EnableCantBeUsedInteractionText(string text)
    {
        StartCoroutine(CantBeUsedInteractionText(text));
    }

    private IEnumerator CantBeUsedInteractionText(string text)
    {
        screenText.text = text;
        screenText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f); // Wait for the specified time
        screenText.gameObject.SetActive(false); // Disable the text
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
            notesText.text = "All notes collected!";
            allNotesCollected = true;
        }
    }

    public void UpdateHealthBar()
    {
        slider.value = Sanity;
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

    public void UpdateSprintText()
    {
        sprintText.text = $"Sprint Energy: {playerMovement.SprintEnergy:F0}%"; // F0 formats it as a whole number (no decimal places)
    }

    private void DrainHealth()
    {
        if (flashlight.isOn)
        {
            Sanity -= healthDrainRate * Time.deltaTime;
        }
    }

    public void CantLeaveText()
    {
        StartCoroutine(CantLeaveEnumerator());
    }

    private IEnumerator CantLeaveEnumerator()
    {
        screenText.gameObject.SetActive(true);
        if (barrier.hasMagicBall == false && allNotesCollected == false)
        {
            screenText.text = $"You can't leave yet! The Magic item and {6 - noteInventoryManager.NotesCollected} notes are missing";
        }
        else if (barrier.hasMagicBall == false)
        {
            screenText.text = "You can't leave yet! The magic item is missing!";
        }
        else if (allNotesCollected == false)
        {
            if (6 - noteInventoryManager.NotesCollected == 1)
            {
                screenText.text = "You can't leave yet! A note is missing!";
            }
            else
            {
                screenText.text = $"You can't leave yet! {6 - noteInventoryManager.NotesCollected} notes are missing!";
            }
        }
        yield return new WaitForSeconds(5f); // Wait for the specified time
        screenText.gameObject.SetActive(false); // Disable the text
    }

    public void RunText()
    {
        StartCoroutine(RunTextEnumator());
    }

    private IEnumerator RunTextEnumator()
    {
        creepyJumpScareSound.Play();
        screenText.gameObject.SetActive(true);
        if (keyInventoryManager.keyCount == 6)
        {
            screenText.text = "IT'S COMING FOR YOU. RUN NOW! YOU HAVE THE KEY FOR THE FRONT DOOR";
        }
        else
        {
            screenText.text = "IT'S COMING FOR YOU. RUN NOW! GRAB THE KEY FOR THE FRONT DOOR";
        }
        yield return new WaitForSeconds(5f); // Wait for the specified time
        screenText.gameObject.SetActive(false); // Disable the text
    }

    public void GameDone()
    {
        gameDonePanel.SetActive(true);
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Forest");
    }
}
