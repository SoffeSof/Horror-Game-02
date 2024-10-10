using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private float mouseSensitivity = 100f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");
    }
}
