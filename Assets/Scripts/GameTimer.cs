using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timerDuration = 60f;  // Total time in seconds
    public Text timerText;
    public Text hiderText;  // UI text to display the number of hiders remaining
    private float timeRemaining;
    private int hidersRemaining;

    public string aiLayerName = "obstacleLayer"; // Name of the layer assigned to the AI player
    public AudioSource[] allAudioSources; // Array of all other audio sources to pause
    public AudioSource specificAudioSource; // The audio source to keep playing

    public GameObject glitchAnim; // Glitch Animation
    public GameObject glitchAudio;

    void Start()
    {
        glitchAnim.gameObject.SetActive(false);
        glitchAudio.gameObject.SetActive(false);
        timeRemaining = timerDuration;

        // Dynamically get the number of hiders at the start of the game
        hidersRemaining = GetHiderCount();
        UpdateTimerDisplay();
        UpdateHiderDisplay();
    }

    void Update()
    {
        // Decrease the timer
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            // If the timer reaches zero, end the game
            EndGame("Time's up!");
        }

        // Check if all hiders are tagged
        if (hidersRemaining <= 0)
        {
            EndGame("All hiders are tagged!");
        }
    }

    // Update the UI to display the current time
    private void UpdateTimerDisplay()
    {
        timerText.text = Mathf.Ceil(timeRemaining).ToString(); // Round to whole number
    }

    // Update the UI to display the remaining hiders
    private void UpdateHiderDisplay()
    {
        hiderText.text = "Hiders Remaining: " + hidersRemaining.ToString();
    }

    // Call this method when a hider is tagged
    public void HiderTagged()
    {
        if (hidersRemaining > 0)
        {
            hidersRemaining--;
            UpdateHiderDisplay();
        }
    }

    public void EndGame(string message)
    {
        StartCoroutine(EndGameWithDelay(message));
    }

    private IEnumerator EndGameWithDelay(string message)
    {
        // Activate the glitch animation
        glitchAnim.gameObject.SetActive(true);
        glitchAudio.gameObject.SetActive(true);

        Debug.Log("Game Over! " + message);

        // Freeze the game by setting time scale to 0
        Time.timeScale = 0;

        // Wait for 2 seconds in real time (not affected by Time.timeScale)
        yield return new WaitForSecondsRealtime(2f);

        // Unfreeze the game (optional, if you want to reset time scale)
        Time.timeScale = 1;

        // Load the GameOverScene after the delay
        SceneManager.LoadScene("GameOverScene"); // Make sure this scene exists
    }

    // Helper method to find an object by layer
    private GameObject FindObjectByLayer(string layerName)
    {
        // Get the layer ID from the layer name
        int layerID = LayerMask.NameToLayer(layerName);

        if (layerID == -1)
        {
            Debug.LogError("Layer '" + layerName + "' does not exist!");
            return null;
        }

        // Find all objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Search for an object on the specified layer
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layerID)
            {
                return obj; // Return the first object found on the layer
            }
        }

        return null; // No object found on the layer
    }

    // Helper method to pause all audio sources except the specified one
    private void PauseAllAudioExcept(AudioSource exception)
    {
        // Find all audio sources in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource != exception && audioSource.isPlaying)
            {
                audioSource.Pause(); // Pause all other audio sources
            }
        }
    }

    // Helper method to get the number of hiders in the scene
    private int GetHiderCount()
    {
        // Find all GameObjects with the "Hider" tag
        GameObject[] hiders = GameObject.FindGameObjectsWithTag("Hider");
        return hiders.Length;
    }
}