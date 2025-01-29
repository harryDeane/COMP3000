using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFleeBehavior : MonoBehaviour
{
    public float fleeDistance = 5f; // Distance at which the bird flees
    public float flySpeed = 3f; // Speed at which the bird flies away
    public Vector3 fleeDirection = Vector3.up; // Direction the bird flies
    public AudioClip fleeSound; // Sound effect to play when fleeing

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
        if (other.CompareTag("Hider")) // Ensure the player has the "Player" tag
        {
            isFleeing = true; // Start fleeing
            
            audioSource.clip = fleeSound;
            audioSource.loop = true; // Loop the sound
            PlayFleeSound(); // Play the sound effect
        }
    }

    private void PlayFleeSound()
    {
        if (fleeSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fleeSound); // Play the sound effect
        }
        else
        {
            Debug.LogWarning("Flee sound or AudioSource is missing.");
        }
    }
}