using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=_QajrabyTJc&t=10s tutorial by Brackeys
    //https://www.youtube.com/watch?v=yTKsQ7jucxg
    
    // Player Controller
    public CharacterController controller;

    // Movement Variables
    private float movementSpeed = 5f;
    private float sprintSpeed = 11f;
    private float currentSpeed;

    // Gravity and Jump Variables
    private float gravity = -9.81f * 3;
    private float jumpHeight = 2f;
    private Vector3 velocity;

    // Ground Detection Variables
    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded = true;

    // SprintEnergy Variables
    public float maxEnergy = 50f;  // Maximum energy
    public float energyDrainRate = 10f;  // SprintEnergy consumed per second while sprinting
    public float energyRegenerationRate = 5f;  // SprintEnergy regained per second when not sprinting
    public float energyMin = 0f;  // SprintEnergy threshold below which sprinting stops
    public float energyThreshold = 10f;  // SprintEnergy threshold below which sprinting stops
    public bool canSprint = true;  // Whether the player can sprint

    [SerializeField] private float _sprintEnergy = 50f; // Private backing field

    public float SprintEnergy
    {
        get { return _sprintEnergy; }
        set { 
                _sprintEnergy = Mathf.Clamp(value, 0f, 100f);  
                HUDController.Instance.UpdateSprintText();
            } // Clamps the value between 0 and 100
    }

    void Start()
    {   
        currentSpeed = movementSpeed;
        SprintEnergy = maxEnergy;  // Initialize energy to max
        HUDController.Instance.GetReferences();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (Input.GetKeyUp(KeyCode.LeftShift) && SprintEnergy <= energyRegenerationRate)
        {
            canSprint = false; // Stop sprinting and reset speed to normal
        }
        else if (Input.GetKey(KeyCode.LeftShift) && SprintEnergy >= energyThreshold)
        {
            canSprint = true;
        }
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && SprintEnergy > energyMin && canSprint) // Sprint
        {
            currentSpeed = sprintSpeed; // Increase speed for sprinting
            SprintEnergy -= energyDrainRate * Time.deltaTime; // Drain energy while sprinting
        }
        else
        {
            currentSpeed = movementSpeed; // Reset speed to normal
            // Regenerate energy slowly when not sprinting
            if (SprintEnergy < maxEnergy)
            {
                SprintEnergy += energyRegenerationRate * Time.deltaTime;
                SprintEnergy = Mathf.Min(SprintEnergy, maxEnergy);  // Cap energy at max
            }
        }

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
