using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIHearing : MonoBehaviour
{
    public float hearingRange = 10f; // Maximum distance for hearing the player
    public float loudnessThreshold = 0.1f; // Loudness threshold to react
    public float wanderRadius = 20f; // Radius for random wandering
    public float wanderTimer = 5f; // Time between random movements
    public float chaseDuration = 3f; // Time to chase the player before stopping if no sound is heard

    private Transform player; // Reference to the VR player
    private NavMeshAgent navMeshAgent; // NavMeshAgent for AI movement
    private float timer; // Timer for random wandering
    private bool isChasingPlayer = false; // Whether the AI is chasing the player
    private bool isChasingSound = false; // Whether the AI is chasing a sound
    private float chaseCooldownTimer; // Timer to track how long the AI has been chasing without hearing loud sounds
    private Vector3 targetPosition; // Position of the sound the AI is chasing

    public Animator aiAnimator;

    public AudioSource chasingVoice; // The AudioSource for the voiceline

    void Start()
    {
        // Find the VR player by tag
        player = GameObject.FindGameObjectWithTag("Hider")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found. Ensure the player has the 'Player' tag.");
        }

        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the AI player.");
        }

        // Initialize the timer
        timer = wanderTimer;
    }

    void Update()
    {
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned.");
            return;
        }

        if (isChasingSound)
        {
            // Set the "IsOpen" parameter to true for both animators
            if (aiAnimator != null)
            {
                aiAnimator.SetBool("isWalking", true);
            }
            // If chasing a sound, move toward the target position
            navMeshAgent.SetDestination(targetPosition);
            Debug.Log("AI is moving toward the sound at: " + targetPosition);

            // Update the chase cooldown timer
            chaseCooldownTimer -= Time.deltaTime;

            // If the cooldown timer reaches 0, stop chasing
            if (chaseCooldownTimer <= 0)
            {
                Debug.Log("AI stopped chasing because it didn't hear new sounds for " + chaseDuration + " seconds.");
                isChasingSound = false;
                if (aiAnimator != null)
                {
                    aiAnimator.SetBool("isWalking", false);
                }
                WanderRandomly(); // Start wandering again
            }
        }

        if (isChasingPlayer)
        {
            // If chasing the player, move toward them
            if (player != null)
            {
                navMeshAgent.SetDestination(player.position);
                Debug.Log("AI is moving toward the player.");
                chasingVoice.Play();
                if (aiAnimator != null)
                {
                    aiAnimator.SetBool("isWalking", true);
                }

            }
            else
            {
                Debug.LogWarning("Player reference is null.");
            }

            // Update the chase cooldown timer
            chaseCooldownTimer -= Time.deltaTime;

            // If the cooldown timer reaches 0, stop chasing
            if (chaseCooldownTimer <= 0)
            {
                Debug.Log("AI stopped chasing because it didn't hear the player for " + chaseDuration + " seconds.");
                isChasingPlayer = false;
                if (aiAnimator != null)
                {
                    aiAnimator.SetBool("isWalking", false);
                }
                WanderRandomly(); // Start wandering again
            }
        }
        else
        {
            // Otherwise, wander randomly
            timer += Time.deltaTime;
            if (timer >= wanderTimer)
            {
                WanderRandomly();
                timer = 0;
            }
        }
    }

    // Called when a sound is detected
    public void OnSoundDetected(Vector3 soundPosition, float loudness)
    {
        // Check if the loudness exceeds the threshold and the sound is within hearing range
        float distanceToSound = Vector3.Distance(transform.position, soundPosition);
        if (loudness > loudnessThreshold && distanceToSound <= hearingRange)
        {
            isChasingSound = true; // Start chasing the sound
            chaseCooldownTimer = chaseDuration; // Reset the chase cooldown timer
            targetPosition = soundPosition; // Set the target position to the sound's position
            Debug.Log("AI heard a sound and is chasing it for " + chaseDuration + " seconds.");
        }
    }

    // Called when the VR player speaks loudly
    public void OnLoudSpeechDetected(float loudness)
    {
        // Check if the loudness exceeds the threshold and the player is within hearing range
        if (loudness > loudnessThreshold && IsPlayerInHearingRange())
        {
            isChasingPlayer = true; // Start chasing the player
            chaseCooldownTimer = chaseDuration; // Reset the chase cooldown timer
            Debug.Log("AI heard the player and is chasing for " + chaseDuration + " seconds.");
        }
    }

    // Check if the player is within hearing range
    private bool IsPlayerInHearingRange()
    {
        if (player == null) return false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= hearingRange;
    }

    // Make the AI wander randomly within a radius
    private void WanderRandomly()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        bool foundPosition = NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas);

        if (foundPosition)
        {
            navMeshAgent.SetDestination(hit.position);
            Debug.Log("AI is wandering to: " + hit.position);
        }
        else
        {
            Debug.LogWarning("Could not find a valid NavMesh position to wander to.");
        }
    }

    // Optional: Stop chasing the player if they move out of range
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hider"))
        {
            Debug.Log("Player moved out of range. AI stopped chasing.");
            isChasingPlayer = false; // Stop chasing
            WanderRandomly(); // Start wandering again
        }
    }
}
