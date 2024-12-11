using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBottleCollision : MonoBehaviour
{
    public GameObject pulseVisualPrefab;
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play the sound only on significant impacts
        if (collision.relativeVelocity.magnitude > 2f)
        {
            audioSource.Play();
            EmitPulse();
        }
    }

    private void EmitPulse()
    {
        if (pulseVisualPrefab == null)
        {
            Debug.LogError("Pulse Visual Prefab is not assigned!");
            return;
        }

        // Instantiate the visual pulse prefab at the object's position
        GameObject pulse = Instantiate(pulseVisualPrefab, transform.position, Quaternion.identity);


        // Destroy the pulse after a short duration 
        Destroy(pulse, 2f);

        

        Debug.Log("Visual pulse emitted from the object!");
    }
}
