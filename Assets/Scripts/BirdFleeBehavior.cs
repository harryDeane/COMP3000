using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFleeBehavior : MonoBehaviour
{
    public float fleeDistance = 5f; // Distance at which the bird flees
    public float flySpeed = 3f; // Speed at which the bird flies away
    public Vector3 fleeDirection = Vector3.up; // Direction the bird flies
    public AudioClip fleeSound; // Sound effect to play when fleeing
    public SoundEmitter soundEmitter; // Reference to the SoundEmitter component

    private bool isFleeing = false;
    private Vector3 initialPosition;
    private AudioSource audioSource;

    void Start()
    {
        initialPosition = transform.position; // Store the bird's initial position
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found on the bird.");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure the SoundEmitter component is assigned
        if (soundEmitter == null)
        {
            Debug.LogWarning("SoundEmitter is not assigned to the bird.");
        }
    }

    void Update()
    {
        if (isFleeing)
        {
            // Move the bird in the flee direction
            transform.Translate(fleeDirection * flySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hider")) // Ensure the player has the "Hider" tag
        {
            isFleeing = true; // Start fleeing

            // Play the flee sound
            if (fleeSound != null && audioSource != null)
            {
                audioSource.clip = fleeSound;
                audioSource.loop = true; // Loop the sound
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("Flee sound or AudioSource is missing.");
            }

            // Emit a sound to alert the AI player(s)
            if (soundEmitter != null)
            {
                soundEmitter.EmitSound();
                Debug.Log("Bird emitted sound as it fled.");
            }
            else
            {
                Debug.LogWarning("SoundEmitter is not assigned.");
            }
        }
    }
}