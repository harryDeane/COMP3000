using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class Echolocation2 : MonoBehaviour
{
    public GameObject soundWavePrefab;    // Assign the wave prefab in the Inspector
    public GameObject pulseVisualPrefab; // Assign the pulse visual prefab
    public float cooldownTime = 5f;      // Cooldown between sound wave emissions
    public float minWaveScale = 0.5f;    // Minimum scale of the sound wave (for quiet speech)
    public float maxWaveScale = 5f;     // Maximum scale of the sound wave (for loud speech)
    public float loudnessToScaleFactor = 10f; // Factor to convert loudness to wave scale
    public float quietLoudnessThreshold = 0.01f; // Threshold for detecting quiet speech

    private float nextEmissionTime = 0f;
    private PlayerControls playerControls; // Reference to the Input System actions
    private MicrophoneInput microphoneInput; // Reference to the MicrophoneInput script

    void Awake()
    {
        // Initialize the PlayerControls
        playerControls = new PlayerControls();

        // Get the MicrophoneInput component
        microphoneInput = FindObjectOfType<MicrophoneInput>();
        if (microphoneInput == null)
        {
            Debug.LogError("MicrophoneInput script not found in the scene!");
        }
    }

    void OnEnable()
    {
        // Enable the PlayerControls and subscribe to the Echolocation action
        playerControls.Enable();
        playerControls.Gameplay.Echolocation.performed += OnEcholocation; // Bind action

        // Subscribe to the loud speech event from MicrophoneInput
        if (microphoneInput != null)
        {
            microphoneInput.OnLoudSpeechDetected += OnLoudSpeechDetected;
        }
    }

    void OnDisable()
    {
        // Disable the PlayerControls and unsubscribe from the Echolocation action
        playerControls.Gameplay.Echolocation.performed -= OnEcholocation;
        playerControls.Disable();

        // Unsubscribe from the loud speech event
        if (microphoneInput != null)
        {
            microphoneInput.OnLoudSpeechDetected -= OnLoudSpeechDetected;
        }
    }

    // Triggered when the Echolocation action is performed
    private void OnEcholocation(InputAction.CallbackContext context)
    {
        if (Time.time >= nextEmissionTime)
        {
            EmitWave(minWaveScale); // Emit a wave with the minimum scale
        }
    }

    // Triggered when speech is detected (quiet or loud)
    private void OnLoudSpeechDetected(float loudness)
    {
        if (Time.time >= nextEmissionTime)
        {
            // Calculate the wave scale based on loudness
            float waveScale = Mathf.Clamp(loudness * loudnessToScaleFactor, minWaveScale, maxWaveScale);
            EmitWave(waveScale);
        }
    }

    private void EmitWave(float waveScale)
    {
        if (pulseVisualPrefab == null)
        {
            Debug.LogError("Pulse Visual Prefab is not assigned!");
            return;
        }
        else
        {
            // Instantiate the sound wave at the player's position
            GameObject soundWave = Instantiate(soundWavePrefab, transform.position, Quaternion.identity);
            soundWave.transform.localScale = Vector3.one * waveScale;

            // Instantiate the visual pulse at the player's position
            Instantiate(pulseVisualPrefab, transform.position, Quaternion.identity);

            // Set the next allowable emission time
            nextEmissionTime = Time.time + cooldownTime;

            Debug.Log($"Sound wave emitted with scale: {waveScale}");
        }
    }
}