using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverProgress : MonoBehaviour
{
    [SerializeField] private Transform visualObject; // The object to fill (e.g., a scale or progress bar)
    [SerializeField] private HingeJoint hingeJoint; // The hinge joint for the lever
    [SerializeField] private float completionThreshold = 45f; // Angle to consider "fully pulled"
    public bool IsComplete { get; private set; } = false; // Public read-only property

    private void Update()
    {
        // Check the lever's angle
        float currentAngle = hingeJoint.angle;

        // Calculate the progress percentage
        float progress = Mathf.InverseLerp(0, completionThreshold, Mathf.Abs(currentAngle));

        // Update the visual object's scale or fill
        if (visualObject != null)
        {
            Vector3 scale = visualObject.localScale;
            scale.y = progress;
            visualObject.localScale = scale;
        }

        // Check if lever is fully pulled
        if (progress >= 1.0f && !IsComplete)
        {
            IsComplete = true;
            OnLeverFullyPulled();
        }
    }

    private void OnLeverFullyPulled()
    {
        Debug.Log($"{name} lever pulled!");
        // Trigger other actions, e.g., send event to manager
    }
}
