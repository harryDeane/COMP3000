using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance for easy access
    public Text scoreText; // UI Text to display the score
    private int score = 0; // Current score

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to add points to the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreDisplay();
    }

    // Method to update the score display
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Method to get the current score
    public int GetScore()
    {
        return score;
    }
}