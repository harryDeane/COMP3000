using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // For Action

public class MicrophoneInput : MonoBehaviour
{
    public float quietLoudnessThreshold => GameSettings.Instance.CurrentSensitivity; // Threshold for detecting quiet speech
    private AIHearing aiHearing; // Reference to the AIHearing script

    public event Action<float> OnLoudSpeechDetected; // Event for speech detection (quiet or loud)

    private AudioClip microphoneClip;
    private string microphoneDevice;

    private bool isSearchingForAI = true; // Flag to indicate if we're still searching for the AI player

    void Start()
    {
        // Start searching for the AI player
        StartCoroutine(FindAIPlayer());

        // Get the default microphone device
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone found.");
            return;
        }

        microphoneDevice = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneDevice, true, 1, 44100); // Start recording
        if (microphoneClip == null)
        {
            Debug.LogError("Failed to start microphone recording.");
        }
    }

    void Update()
    {
        // Only process microphone input if the AI player has been found
        if (!isSearchingForAI)
        {
            float loudness = GetMicrophoneLoudness();
            //Debug.Log("Microphone Loudness: " + loudness); // Log the loudness for debugging

            if (loudness > quietLoudnessThreshold)
            {
                Debug.Log("Player is speaking: " + loudness);
                OnLoudSpeechDetected?.Invoke(loudness); // Trigger the event for any speech above the quiet threshold

                // Notify the AI if the reference is valid
                if (aiHearing != null)
                {
                    aiHearing.OnLoudSpeechDetected(loudness);
                }
                else
                {
                    Debug.LogWarning("AIHearing reference is null.");
                }
            }
        }
    }

    // Coroutine to find the AI player
    private IEnumerator FindAIPlayer()
    {
        while (isSearchingForAI)
        {
            GameObject aiPlayer = GameObject.FindGameObjectWithTag("Seeker");
            if (aiPlayer != null)
            {
                aiHearing = aiPlayer.GetComponent<AIHearing>();
                if (aiHearing != null)
                {
                    Debug.Log("AI player found and AIHearing component assigned.");
                    isSearchingForAI = false; // Stop searching once the AI player is found
                }
                else
                {
                    Debug.LogError("AIHearing component not found on the AI player.");
                }
            }
            else
            {
                Debug.Log("AI player not found yet. Retrying in 1 second...");
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second before retrying
        }
    }

    float GetMicrophoneLoudness()
    {
        if (microphoneClip == null) return 0;

        int samplePosition = Microphone.GetPosition(microphoneDevice);
        float[] samples = new float[microphoneClip.samples * microphoneClip.channels];
        microphoneClip.GetData(samples, 0);

        // Calculate the loudness (RMS value)
        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        return Mathf.Sqrt(sum / samples.Length);
    }

    void OnDestroy()
    {
        if (Microphone.IsRecording(microphoneDevice))
        {
            Microphone.End(microphoneDevice); // Stop recording when the object is destroyed
        }
    }
}