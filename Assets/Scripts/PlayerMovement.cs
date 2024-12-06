using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=_QajrabyTJc&t=10s tutorial by Brackeys
    
    // Player Controller (CharacterController component used for movement)
    public CharacterController controller;

    // Movement Variables
    private float movementSpeed = 5f; // Normal movement speed
    private float sprintSpeed = 11f; // Speed while sprinting
    private float currentSpeed;// Current speed (adjusted based on sprinting)

    // Gravity and Jump Variables
    private float gravity = -9.81f * 3; // Gravity force, adjusted for better movement
    private float jumpHeight = 2f; // Jump height (how high the player can jump)
    private Vector3 velocity; // The velocity, used for gravity and jumping

    // Ground Detection Variables
    public Transform groundCheck; // Position to check if player is grounded
    private float groundDistance = 0.4f; // Distance for the ground check sphere
    public LayerMask groundMask; // Layer to identify what is considered "ground"
    private bool isGrounded = true; // Whether the player is grounded or not

    // SprintEnergy Variables
    public float maxEnergy = 50f; // Maximum amount of sprint energy
    public float energyDrainRate = 5f; // Rate at which sprint energy is drained per second
    public float energyRegenerationRate = 5f; // Rate at which energy regenerates when not sprinting
    public float energyMin = 0f; // Minimum energy required for sprinting
    public float energyThreshold = 10f; // Threshold energy below which sprinting stops
    public bool canSprint = true; // Whether the player can sprint or not


    [SerializeField] private float _sprintEnergy = 50f; // Private backing field for sprint energy

    public float SprintEnergy // Property for SprintEnergy with clamping and HUD update
    {
        get { return _sprintEnergy; }
        set { 
                _sprintEnergy = Mathf.Clamp(value, 0f, 100f);   //Ensure energy is between 0 and 100
                HUDController.Instance.UpdateSprintText(); // Update HUD with current sprint energy
            }
    }

    void Start()
    {   
        currentSpeed = movementSpeed; // Set initial movement speed
        SprintEnergy = maxEnergy;  // Initialize energy to max
        HUDController.Instance.GetReferences(); // Get references for HUDController for the new scene.
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Check if player is grounded by checking a sphere cast at groundCheck position
        if(isGrounded && velocity.y < 0) // If player is grounded and falling, reset the vertical velocity
        {
            velocity.y = -2f; // Reset velocity to a small value to prevent falling through the ground
        }

        //Get player input for movement (WASD or arrow keys)
        float x = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow for left/right movement
        float z = Input.GetAxis("Vertical"); // W/S or Up/Down Arrow for forward/backward movement

        // Handle sprinting behavior based on input and energy levels
        if (Input.GetKeyUp(KeyCode.LeftShift) && SprintEnergy <= energyThreshold) // Stop sprinting if energy is too low
        {
            canSprint = false; // Stop sprinting and reset speed to normal
        }
        else if (Input.GetKey(KeyCode.LeftShift) && SprintEnergy >= energyThreshold) // Allow sprinting if energy is above threshold
        {
            canSprint = true; // Allow sprinting and reset speed to normal
        }
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && SprintEnergy > energyMin && canSprint) //If the player presses Left Shift and has enough energy to sprint and is grounded and can sprint
        {
            currentSpeed = sprintSpeed; // Increase speed for sprinting
            SprintEnergy -= energyDrainRate * Time.deltaTime; // Drain energy while sprinting
        }
        else //If not
        {
            currentSpeed = movementSpeed; // Reset speed to normal
            if (SprintEnergy < maxEnergy)// Regenerate energy slowly when not sprinting
            {
                SprintEnergy += energyRegenerationRate * Time.deltaTime; // Regenerate energy when not sprinting
            }
        }

        // Move the player based on input and speed
        Vector3 move = transform.right * x + transform.forward * z; // Calculate movement direction

        controller.Move(move * currentSpeed * Time.deltaTime);// Apply movement to the playercontroller

        // Jumping behavior
        if(Input.GetButtonDown("Jump") && isGrounded) // If the player presses the jump button and is grounded
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Set jump velocity based on jump height and gravity
        }
        velocity.y += gravity * Time.deltaTime;  // Apply gravity to vertical velocity
        controller.Move(velocity * Time.deltaTime); // Apply gravity and vertical movement to the player

    }
}
