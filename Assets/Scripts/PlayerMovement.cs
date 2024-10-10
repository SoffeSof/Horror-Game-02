using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=_QajrabyTJc&t=10s tutorial by Brackeys
    //https://www.youtube.com/watch?v=yTKsQ7jucxg
    public CharacterController controller;
    
    private float gravity = -9.81f*3;
    private float movementSpeed = 12f;
    private float sprintSpeed = 20f;
    private float crouchSpeed = 6f;
    private float jumpHeight = 2f;
    private Vector3 velocity;

    private float currentSpeed;

    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    private bool isCrouching = false;
    private float normalYScale = 1f;
    private float crouchYScale = 0.5f;
    private float smoothTime = 2f; 
    private Vector3 targetScale; // Target scale for crouching

    void Start()
    {   
        currentSpeed = movementSpeed;
        targetScale = new Vector3(transform.localScale.x, normalYScale, transform.localScale.z);
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded) // Sprint
        {
            currentSpeed = sprintSpeed; // Increase speed for sprinting
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) // Stop sprinting
        {
            currentSpeed = movementSpeed; // Reset speed to normal
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded) // Crouch
        {
            currentSpeed = crouchSpeed; // Decrease speed for crouching
            targetScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); // Set target scale for crouching;
            velocity.y = -10f;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && isGrounded) // Stop crouching
        {
            currentSpeed = movementSpeed; // Reset speed to normal
            targetScale = new Vector3(transform.localScale.x, normalYScale, transform.localScale.z); // Set target scale for crouching;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, smoothTime);

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
