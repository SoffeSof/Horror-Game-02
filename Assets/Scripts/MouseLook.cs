using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //https://www.youtube.com/watch?v=_QajrabyTJc&t=10s tutorial by Brackeys
    
    private float mouseSensitivity = 200f; // Variable to control how sensitive the mouse movement is
    public Transform playerBody; // Reference to the player's body, used for rotating the player horizontally

    private float xRotation = 0f; // Tracks the vertical rotation of the camera (up/down)
    private int topClamp = -90; // The top limit for the vertical rotation (looking up)
    private int bottomClamp = 90; // The bottom limit for the vertical rotation (looking down)
    public bool isInventoryOpen = false; // Flag to check if the inventory is open, preventing mouse look when true

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor in the center of the screen and makes it invisible
    }

    void LateUpdate()
    {
        if (isInventoryOpen == false) // If the inventory is not open, allow mouse look control
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Get the horizontal mouse movement (X-axis) multiplied by sensitivity and frame time to make it frame-rate independent
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // Get the vertical mouse movement (Y-axis) with the same multiplication

            xRotation -= mouseY;  // Decrease the xRotation by the vertical mouse movement to look up and down (inverted Y axis)
            xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp); // Clamps the xRotation between -90 and 90 degrees to prevent the player from over-rotating (looking too far up or down)

            playerBody.Rotate(Vector3.up * mouseX);  // Rotate the player's body around the Y-axis (horizontal axis) based on the mouse's X-axis movement
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate the camera (attached to this script) around the X-axis (vertical axis) to look up and down
        }
    }
}