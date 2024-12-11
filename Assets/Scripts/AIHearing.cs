using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

public class AIHearing : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform
    public float moveSpeed = 5f; // Movement speed of the AI
    public float hearingRange = 10f; // Maximum distance at which the AI can hear the player
    public float randomMoveInterval = 3f; // Time interval to choose a new random position
    public Vector2 mapBounds; // Map boundaries for random movement (assumes a 2D map for simplicity)

    private PhotonVoiceView photonVoiceView;
    private bool isPlayerTalking = false;
    private Vector3 randomTargetPosition; // Target position for random movement
    private float timeToNextRandomMove; // Timer for next random move

    void Start()
    {
        photonVoiceView = playerTransform.GetComponent<PhotonVoiceView>();
        timeToNextRandomMove = randomMoveInterval; // Start the timer for random movement
        SetRandomTargetPosition();
    }

    void Update()
    {
        // Update the timer for random movement
        timeToNextRandomMove -= Time.deltaTime;
        if (timeToNextRandomMove <= 0)
        {
            SetRandomTargetPosition();
            timeToNextRandomMove = randomMoveInterval; // Reset the timer
        }

        // Ensure the PhotonVoiceView component and Recorder are valid
        if (photonVoiceView != null && photonVoiceView.RecorderInUse != null)
        {
            // Check if the player is transmitting (actively speaking)
            isPlayerTalking = photonVoiceView.RecorderInUse.TransmitEnabled;
        }

        // If the player is talking and within range, move towards the player
        if (isPlayerTalking && IsPlayerInRange())
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Otherwise, move towards the random target
            MoveTowardsRandomPosition();
        }
    }

    bool IsPlayerInRange()
    {
        // Calculate the distance between the AI and the player
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Return true if the player is within hearing range
        return distance <= hearingRange;
    }

    void SetRandomTargetPosition()
    {
        // Generate a random position within the specified map bounds
        float randomX = Random.Range(-mapBounds.x, mapBounds.x);
        float randomZ = Random.Range(-mapBounds.y, mapBounds.y);
        randomTargetPosition = new Vector3(randomX, transform.position.y, randomZ); // Keep AI's current Y position
    }

    void MoveTowardsPlayer()
    {
        // Calculate direction to move towards the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void MoveTowardsRandomPosition()
    {
        // Calculate direction to move towards the random target
        Vector3 direction = (randomTargetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // If the AI reaches the target position, stop and choose a new target
        if (Vector3.Distance(transform.position, randomTargetPosition) < 1f) // Threshold for reaching the target
        {
            SetRandomTargetPosition(); // Set a new random target
        }
    }
}
