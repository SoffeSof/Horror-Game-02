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
    private float movementSpeed = 9f;
    private float sprintSpeed = 16f;
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

    void Start()
    {   
        currentSpeed = movementSpeed;
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

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded) // Sprint
        {
            currentSpeed = sprintSpeed; // Increase speed for sprinting
        }
        else
        {
            currentSpeed = movementSpeed; // Reset speed to normal
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
