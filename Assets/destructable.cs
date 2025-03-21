using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Required for VR interactable components

public class Destructable : MonoBehaviour
{
    public GameObject destroyedVersion; // Reference to the destroyed version of the object
    public SoundEmitter soundEmitter; // Reference to the SoundEmitter component
    public AudioClip smashSound; // Sound to play when the bottle smashes
    public float soundVolume = 1.0f; // Volume of the sound

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; // Reference to the VR interactable component
    private Rigidbody rb; // Reference to the Rigidbody component
    private bool isThrown = false; // Track if the object has been thrown
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get the XRGrabInteractable component
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Subscribe to the event when the object is released
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnReleased(SelectExitEventArgs args)
    {
        // Mark the object as thrown
        isThrown = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object is thrown and collides with something
        if (isThrown && rb.velocity.magnitude > 2.0f) // Adjust the velocity threshold as needed
        {
            // Play the smash sound
            PlaySmashSound();

            // Emit the sound
            if (soundEmitter != null)
            {
                soundEmitter.EmitSound();
            }
            else
            {
                Debug.LogWarning("No SoundEmitter assigned to the Destructable script.");
            }

            // Instantiate the destroyed version at the same position and rotation
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            // Destroy the original object
            Destroy(gameObject);
        }
    }

    void PlaySmashSound()
    {
        // Check if a sound is assigned
        if (smashSound != null)
        {
            // Play the sound effect
            audioSource.PlayOneShot(smashSound, soundVolume);
        }
        else
        {
            Debug.LogWarning("No smash sound assigned to the Destructable script.");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to avoid memory leaks
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
}