using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManHunt : MonoBehaviour
{
    public enum Team
    {
        Seeker,
        Hider
    }

    public Team currentTeam = Team.Seeker;  // Default to Seeker
    public string hiderTag = "Hider";  // Tag for hider players
    public string seekerTag = "Seeker";  // Tag for seeker players

    // Reference to other components or objects
    private Collider mainCollider;  // Reference to the main non-trigger collider
    private Collider triggerCollider;  // Reference to the trigger collider
    private Renderer playerRenderer;

    void Start()
    {
        // Find and separate the colliders for different purposes
        mainCollider = GetComponent<Collider>();  // Main collider (non-trigger)
        triggerCollider = transform.Find("TriggerCollider")?.GetComponent<Collider>();  // Optional, if you separate colliders

        // Optional: Set color based on team for visualization (for debugging)
        playerRenderer = GetComponent<Renderer>();
        UpdatePlayerAppearance();
    }

    void OnTriggerEnter(Collider other)
    {
        // Ensure the team transition only happens with the trigger
        if (currentTeam == Team.Seeker && other.CompareTag(hiderTag))
        {
            TransitionToSeeker();
        }
        else if (currentTeam == Team.Hider && other.CompareTag(seekerTag))
        {
            TransitionToSeeker();
        }
    }

    void TransitionToSeeker()
    {
        currentTeam = Team.Seeker;
        UpdatePlayerAppearance();
        Debug.Log("Player is now a Seeker");
    }

    void TransitionToHider()
    {
        currentTeam = Team.Hider;
        UpdatePlayerAppearance();
        Debug.Log("Player is now a Hider");
    }

    void UpdatePlayerAppearance()
    {
        if (currentTeam == Team.Seeker)
        {
            playerRenderer.material.color = Color.green;  // Example color for Seeker
        }
        else if (currentTeam == Team.Hider)
        {
            playerRenderer.material.color = Color.grey;  // Example color for Hider
        }
    }
}