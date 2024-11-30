using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timerDuration = 60f;  // Total time in seconds
    public Text timerText;             
    private float timeRemaining;

    void Start()
    {
        timeRemaining = timerDuration;
        UpdateTimerDisplay();
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
            EndGame();
        }
    }

    // Update the UI to display the current time
    private void UpdateTimerDisplay()
    {
        timerText.text = Mathf.Ceil(timeRemaining).ToString(); // Round to whole number
    }

    // End the game when timer runs out
    private void EndGame()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOverScene");  // Make sure this scene exists
    }
}
