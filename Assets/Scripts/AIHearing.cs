using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIHearing : MonoBehaviour
{
    public float hearingRange = 1000f; // Maximum distance for hearing the player
    public float loudnessThreshold = 0.01f; // Loudness threshold to react
    public float wanderRadius = 20f; // Radius for random wandering
    public float wanderTimer = 5f; // Time between random movements
    public float chaseDuration = 7f; // Time to chase the player before stopping if no sound is heard

    public float soundLoudnessToDistanceMultiplier = 100f;

    public float aiSpeedThreshold = 1f; // Speed threshold to consider the AI as moving

    private Transform player; // Reference to the VR player
    private NavMeshAgent navMeshAgent; // NavMeshAgent for AI movement
    private float timer; // Timer for random wandering
    private bool isChasingPlayer = false; // Whether the AI is chasing the player
    private bool isChasingSound = false; // Whether the AI is chasing a sound
    private float chaseCooldownTimer; // Timer to track how long the AI has been chasing without hearing loud sounds
    private Vector3 targetPosition; // Position of the sound the AI is chasing
    private Vector3 previousPosition; // Previous position of the AI to calculate speed

    public Animator aiAnimator;

    public AudioSource chasingVoice; // The AudioSource for the voiceline

    void Start()
    {
        // Find the VR player by tag
        player = GameObject.FindGameObjectWithTag("Hider")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found. Ensure the player has the 'Hider' tag.");
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

        // Calculate the AI's speed
        float aiSpeed = (transform.position - previousPosition).magnitude / Time.deltaTime;
        previousPosition = transform.position;

        // Update animations based on AI's speed
        if (aiSpeed > aiSpeedThreshold)
        {
            aiAnimator.SetBool("IsMoving", true); // AI is moving fast
        }
        else
        {
            aiAnimator.SetBool("IsMoving", false); // AI is idle or moving slowly
        }


        if (isChasingSound)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // Keep updating the destination to the target position
            navMeshAgent.SetDestination(targetPosition);
            Debug.Log("AI is moving toward the sound at: " + targetPosition);

            // Check if we've reached the sound location
            if (distanceToTarget <= navMeshAgent.stoppingDistance + 0.5f) // + buffer to account for slight overshoot
            {
                Debug.Log("AI reached the sound location.");
                isChasingSound = false;

                // Optionally pause or investigate here...

                WanderRandomly(); // Resume wandering or some idle behavior
            }
        }

        if (isChasingPlayer)
        {
            // If chasing the player, move toward them
            if (player != null)
            {
                navMeshAgent.SetDestination(player.position);
                Debug.Log("AI is moving toward the player.");
                

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
        float maxDistanceHeard = loudness * soundLoudnessToDistanceMultiplier;
        if (distanceToSound <= maxDistanceHeard)
        {
            isChasingSound = true;
            targetPosition = soundPosition;
            Debug.Log("AI heard a sound and is chasing it.");
        }

    }

    // Called when the VR player speaks loudly
    public void OnLoudSpeechDetected(float loudness)
    {
        Debug.Log("AIHearing: Loud speech detected with loudness: " + loudness);
        if (loudness > loudnessThreshold)
        {
            chasingVoice.Play();
            Debug.Log("AIHearing: Loudness and range conditions met. Starting chase.");
            isChasingPlayer = true; // Start chasing the player
            chaseCooldownTimer = chaseDuration; // Reset the chase cooldown timer
        }
    }

    // Check if the player is within hearing range
    private bool IsPlayerInHearingRange()
    {
        if (player == null) return false;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Debug.Log("Distance to player: " + distanceToPlayer);
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
    
}
