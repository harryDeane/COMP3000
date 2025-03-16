using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public float expansionSpeed = 5f;   // Speed of the wave's expansion
    public float maxScale = 5f;        // Maximum scale for the wave
    public float highlightDuration = 1f; // Duration to highlight objects
    public LayerMask obstacleLayer;     // Layer mask to detect obstacles

    private bool shouldExpand = true;   // Determines whether the wave should keep expanding
    private SphereCollider sphereCollider;
    private ParticleSystem particleSystem;

    void Start()
    {
        // Ensure the collider matches the scale of the wave
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.radius = transform.localScale.x / 2f;
        }

        // Get the ParticleSystem component
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (shouldExpand)
        {
            // Expand the wave
            transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;

            // Update the collider radius to match the wave scale
            if (sphereCollider != null)
            {
                sphereCollider.radius = transform.localScale.x / 2f;
            }

            // Destroy the wave if it reaches maximum scale
            if (transform.localScale.x >= maxScale)
            {
                shouldExpand = false; // Stop expanding
                StartCoroutine(DestroyAfterParticles());
            }
        }
    }

    IEnumerator DestroyAfterParticles()
    {
        // Stop the particle system from emitting new particles
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }

        // Wait for the remaining particles to die out
        if (particleSystem != null)
        {
            yield return new WaitForSeconds(particleSystem.main.startLifetime.constantMax);
        }
        else
        {
            yield return null;
        }

        // Destroy the game object
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        EchoInteractable interactable = other.GetComponent<EchoInteractable>();
        if (interactable != null)
        {
            // Line-of-sight check
            Vector3 directionToTarget = other.transform.position - transform.position;
            if (Physics.Raycast(transform.position, directionToTarget.normalized, out RaycastHit hit, directionToTarget.magnitude, obstacleLayer))
            {
                if (hit.collider == other)
                {
                    // Highlight the specific surface hit
                    interactable.HighlightSurface(hit.point, hit.normal, highlightDuration);
                    Debug.Log($"Illuminated {other.name} at {hit.point}");
                }
                else
                {
                    Debug.Log($"{other.name} is blocked by {hit.collider.name}");
                }
            }
        }
    }
}