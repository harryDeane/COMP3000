using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;    // Sensitivity for mouse look
    public Transform playerBody;             // The player's body (for rotating the player)
    private float xRotation = 0f;            // Store the camera's vertical rotation

    private PlayerControls controls;         // Reference to the Input System controls
    private Vector2 lookInput;               // Store the mouse input (for movement)

    void Awake()
    {
        // Initialize the Input System controls
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        // Enable the controls when the script is enabled
        controls.Enable();

        // Bind the mouse movement input to the lookInput variable
        controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => lookInput = Vector2.zero; // Reset when not moving
    }

    void OnDisable()
    {
        // Disable the controls when the script is disabled
        controls.Disable();
    }

    void Update()
    {
        // Horizontal rotation (Y-axis rotation for the player)
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        playerBody.Rotate(Vector3.up * mouseX);  // Rotate the player body left/right

        // Vertical rotation (X-axis rotation for the camera)
        xRotation -= lookInput.y * mouseSensitivity * Time.deltaTime; // Look up/down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // Clamp the vertical rotation to prevent over-rotation

        // Apply the vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);  // Rotate the camera up/down
    }
}

