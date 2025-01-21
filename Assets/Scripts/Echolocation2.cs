using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Echolocation2 : MonoBehaviour
{
    public GameObject soundWavePrefab;    // Assign the wave prefab in the Inspector
    public GameObject pulseVisualPrefab; // Assign the pulse visual prefab
    public float cooldownTime = 5f;      // Cooldown between sound wave emissions
    private float nextEmissionTime = 0f;

    private PlayerControls playerControls; // Reference to the Input System actions

    void Awake()
    {
        // Initialize the PlayerControls
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        // Enable the PlayerControls and subscribe to the Echolocation action
        playerControls.Enable();
        playerControls.Gameplay.Echolocation.performed += OnEcholocation; // Bind action
    }

    void OnDisable()
    {
        // Disable the PlayerControls and unsubscribe from the Echolocation action
        playerControls.Gameplay.Echolocation.performed -= OnEcholocation;
        playerControls.Disable();
    }

    // Triggered when the Echolocation action is performed
    private void OnEcholocation(InputAction.CallbackContext context)
    {
        if (Time.time >= nextEmissionTime)
        {
            EmitWave();
        }
    }


    private void EmitWave()
    {
        if (pulseVisualPrefab == null)
        {
            Debug.LogError("Pulse Visual Prefab is not assigned!");
            return;
        }
        else
        {
            // Instantiate the sound wave at the player's position
            Instantiate(soundWavePrefab, transform.position, Quaternion.identity);

            // Instantiate the visual pulse at the player's position
            Instantiate(pulseVisualPrefab, transform.position, Quaternion.identity);

            // Set the next allowable emission time
            nextEmissionTime = Time.time + cooldownTime;

            Debug.Log("Sound wave emitted with visuals!");
        }

    }
}


