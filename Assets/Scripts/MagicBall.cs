using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : PickUpItems
{
    public float rotationSpeed = 50f; // Speed of the rotation
    public Vector3 rotationAxis = Vector3.up; // Axis of rotation (default: Y-axis)
    public Barrier barrier;

    // Update is called once per frame
    void Update()
    {
        RotateMagicBall();
    }

    // Method to rotate the MagicBall
    void RotateMagicBall()
    {
        // Rotate the ball around its local axis
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
