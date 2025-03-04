using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class Haptic : MonoBehaviour
{
    public HapticImpulsePlayer hapticImpulsePlayer; // Assign the HapticImpulsePlayer component
    public float amplitude = 0.5f; // Strength of the vibration (0 to 1)
    public float duration = 0.1f; // Duration of the vibration in seconds

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) // Ensure obstacles have the "Obstacle" tag
        {
            PlayHapticFeedback();
        }
    }

    private void PlayHapticFeedback()
    {
        if (hapticImpulsePlayer != null)
        {
            hapticImpulsePlayer.SendHapticImpulse(amplitude, duration);
        }
    }
}