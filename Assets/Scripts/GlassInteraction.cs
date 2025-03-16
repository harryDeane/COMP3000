using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassInteraction : MonoBehaviour
{
    public AudioClip glassCrackSound;  // Glass cracking sound
    public GameObject visualAlertPrefab; // Visual alert prefab (UI Element)
    public float alertDuration = 2f;  // Duration for alert
    public float pulsatingSpeed = 1f;  // Speed of the pulsating effect
    public float maxScale = 1.5f;    // Max scale for pulsating effect
    public float minScale = 1f;      // Min scale for pulsating effect
    public LayerMask viewableLayer;  // Ensure the alert is visible through obstacles
    public int scoreValue = 10;      // Points awarded for interacting with the glass

    private AudioSource audioSource;

    void Start()
    {
        // Set up the audio source if not already attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player steps on the glass
        if (other.CompareTag("Hider"))
        {
            PlayGlassCrackSound();
            ShowVisualAlert();
            // Award points to the player
            ScoreManager.Instance.AddScore(scoreValue);
        }
    }

    private void PlayGlassCrackSound()
    {
        if (glassCrackSound != null)
        {
            audioSource.PlayOneShot(glassCrackSound);
        }
        else
        {
            Debug.LogWarning("Glass crack sound is not assigned!");
        }
    }

    private void ShowVisualAlert()
    {
        if (visualAlertPrefab != null)
        {
            // Instantiate the visual alert at the glass object's position
            GameObject alert = Instantiate(visualAlertPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity); // Position above the glass
            alert.transform.SetParent(transform, true); // Parent it to the glass object so it moves with it (optional)

            // If the alert has a Canvas, make sure it is set to World Space
            Canvas canvas = alert.GetComponentInChildren<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.WorldSpace;
            }

            // Set the visual alert to the specified viewable layer
            alert.layer = viewableLayer;

            // Start pulsating effect
            StartCoroutine(PulsateAlert(alert));
            Destroy(alert, alertDuration); // Destroy after duration
        }
        else
        {
            Debug.LogWarning("Visual alert prefab is not assigned!");
        }
    }


    private IEnumerator PulsateAlert(GameObject alert)
    {
        RectTransform rectTransform = alert.GetComponent<RectTransform>();
        Vector3 originalScale = rectTransform.localScale;

        while (true)
        {
            float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * pulsatingSpeed, 1));
            rectTransform.localScale = originalScale * scale;
            yield return null;
        }
    }
}

