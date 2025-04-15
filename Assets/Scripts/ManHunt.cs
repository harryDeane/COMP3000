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

    // Reference to the GameTimer script
    private GameTimer gameTimer;

    void Start()
    {
        // Find and separate the colliders for different purposes
        mainCollider = GetComponent<Collider>();  // Main collider (non-trigger)
        triggerCollider = transform.Find("TriggerCollider")?.GetComponent<Collider>();  // Optional, if you separate colliders

        // Optional: Set color based on team for visualization (for debugging)
        playerRenderer = GetComponent<Renderer>();
        UpdatePlayerAppearance();

        // Find the GameTimer in the scene
        gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer == null)
        {
            Debug.LogError("GameTimer script not found in the scene!");
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        // Ensure the team transition only happens with the trigger
        if (currentTeam == Team.Seeker && other.CompareTag(hiderTag))
        {
            Debug.Log("Colliding!");
            gameTimer = FindObjectOfType<GameTimer>();
            var hider = other.GetComponent<ManHunt>();
            if (hider != null && hider.currentTeam == Team.Hider)
            {
                hider.TransitionToSeeker();              
                gameTimer.HiderTagged();
                
            }
        }
    }

    void TransitionToSeeker()
    {
        currentTeam = Team.Seeker;
        gameObject.tag = seekerTag; 
        UpdatePlayerAppearance();
        Debug.Log("Player is now a Seeker");
    }


    void TransitionToHider()
    {
        currentTeam = Team.Hider;
        gameObject.tag = hiderTag;
        UpdatePlayerAppearance();
        Debug.Log("Player is now a Hider");
    }

    void UpdatePlayerAppearance()
    {
        if (currentTeam == Team.Seeker)
        {
            playerRenderer.material.color = Color.white;  // Example color for Seeker
        }
        else if (currentTeam == Team.Hider)
        {
            playerRenderer.material.color = Color.grey;  // Example color for Hider
        }
    }
}