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
    public int totalHiders = 5;  // Total number of hiders at the start of the game
    private float timeRemaining;
    private int hidersRemaining;

    void Start()
    {
        timeRemaining = timerDuration;
        hidersRemaining = totalHiders;
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

    // End the game with a specific reason
    private void EndGame(string message)
    {
        Debug.Log("Game Over! " + message);
        SceneManager.LoadScene("GameOverScene");  // Make sure this scene exists
    }
}
