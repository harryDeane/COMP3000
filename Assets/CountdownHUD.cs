using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownHUD : MonoBehaviour
{
    public TMP_Text countdownText; // Reference to the countdown text UI
    public float countdownDuration = 5f; // Duration of the countdown
    public GameObject aiPlayerPrefab; // Reference to the AI player prefab
    public Transform aiSpawnPoint; // Where the AI player will spawn
    public AudioSource countdownBeep; // Sound for each countdown number
    public AudioSource countdownGo; // Sound for "GO!"

    private float countdownTimer;
    private bool isCountingDown = false;

    void Start()
    {
        // Initialize the countdown
        countdownTimer = countdownDuration;
        isCountingDown = true;

        // Ensure the AI player is not spawned yet
        if (aiPlayerPrefab != null)
        {
            aiPlayerPrefab.SetActive(false);
        }
    }

    void Update()
    {
        if (isCountingDown)
        {
            // Update the countdown timer
            countdownTimer -= Time.deltaTime;

            // Update the countdown text
            if (countdownText != null)
            {
                int currentCount = Mathf.CeilToInt(countdownTimer);
                countdownText.text = currentCount.ToString();

                // Play a beep sound for each countdown number
                if (currentCount != Mathf.CeilToInt(countdownTimer + Time.deltaTime))
                {
                    if (countdownBeep != null)
                    {
                        countdownBeep.Play();
                    }
                }
            }

            // End the countdown when the timer reaches 0
            if (countdownTimer <= 0)
            {
                EndCountdown();
            }
        }
    }

    private void EndCountdown()
    {
        isCountingDown = false;

        // Hide the countdown text
        if (countdownText != null)
        {
            countdownText.text = "RUN!";

            if (countdownGo != null)
            {
                countdownGo.Play(); // Play the "GO!" sound
            }
            // Optionally, hide the text after a short delay
            Invoke("HideCountdownText", 1f);
        }

        // Spawn the AI player
        SpawnAIPlayer();
    }

    private void HideCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void SpawnAIPlayer()
    {
        if (aiPlayerPrefab != null && aiSpawnPoint != null)
        {
            // Spawn the AI player at the specified spawn point
            GameObject aiPlayer = Instantiate(aiPlayerPrefab, aiSpawnPoint.position, aiSpawnPoint.rotation);
            aiPlayer.SetActive(true);
            Debug.Log("AI player spawned!");
        }
        else
        {
            Debug.LogWarning("AI player prefab or spawn point is not assigned.");
        }
    }
}