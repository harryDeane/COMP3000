using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverProgress : MonoBehaviour
{
    [SerializeField] private Transform visualObject; // The object to fill (e.g., a scale or progress bar)
    [SerializeField] private HingeJoint hingeJoint; // The hinge joint for the lever
    [SerializeField] private float targetAngle = 180f; // Target angle to hold the lever at
    [SerializeField] private float angleThreshold = 5f; // Allowed deviation from the target angle
    [SerializeField] private float holdDuration = 10f; // Time (in seconds) to hold the lever at the target angle
    public bool IsComplete { get; private set; } = false; // Public read-only property

    private float holdTimer = 0f; // Timer to track how long the lever has been held at the target angle
    public SoundEmitter soundEmitter; // Reference to the SoundEmitter component

    public int scoreValue = 100;      // Points awarded for interacting with the glass

    private void Update()
    {
        if (IsComplete) return; // Stop updating if the lever action is already complete

        // Check the lever's angle
        float currentAngle = hingeJoint.angle;

        // Check if the lever is within the target angle range
        if (Mathf.Abs(currentAngle - targetAngle) <= angleThreshold)
        {
            // Increment the hold timer
            holdTimer += Time.deltaTime;

            if (soundEmitter != null)
            {
                soundEmitter.PlayContinuous();
            }

            // Update the visual object's scale or fill
            if (visualObject != null)
            {
                float progress = Mathf.Clamp01(holdTimer / holdDuration); // Calculate progress as a percentage
                Vector3 scale = visualObject.localScale;
                scale.y = progress;
                visualObject.localScale = scale;
                // Emit a sound to alert the AI player(s)
                if (soundEmitter != null)
                {
                    soundEmitter.EmitSound();

                    Debug.Log("Lever is making sound.");
                }
                else
                {
                    Debug.LogWarning("SoundEmitter is not assigned.");
                }
            }

            // Check if the hold duration has been reached
            if (holdTimer >= holdDuration)
            {
                IsComplete = true;
                OnLeverHeldForDuration();
                // Award points to the player
                ScoreManager.Instance.AddScore(scoreValue);
                // Stop sound when lever is not in position
                if (soundEmitter != null)
                {
                    soundEmitter.StopContinuous();
                }
            }
        }
        else
        {
            // Reset the hold timer if the lever is not within the target angle range
            holdTimer = 0f;

            // Reset the visual object's scale or fill
            if (visualObject != null)
            {
                Vector3 scale = visualObject.localScale;
                scale.y = 0f;
                visualObject.localScale = scale;
            }

            // Stop sound when lever is not in position
            if (soundEmitter != null)
            {
                soundEmitter.StopContinuous();
            }
        }
    }

    private void OnLeverHeldForDuration()
    {
        Debug.Log($"{name} lever held for {holdDuration} seconds!");
       
    }
}