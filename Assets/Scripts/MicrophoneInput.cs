using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // For Action

public class MicrophoneInput : MonoBehaviour
{
    public float quietLoudnessThreshold = 0.01f; // Threshold for detecting quiet speech
    public AIHearing aiHearing; // Reference to the AIHearing script

    public event Action<float> OnLoudSpeechDetected; // Event for speech detection (quiet or loud)

    private AudioClip microphoneClip;
    private string microphoneDevice;

    void Start()
    {
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
        float loudness = GetMicrophoneLoudness();
        //Debug.Log("Microphone Loudness: " + loudness); // Log the loudness for debugging

        if (loudness > quietLoudnessThreshold)
        {
            Debug.Log("Player is speaking: " + loudness);
            OnLoudSpeechDetected?.Invoke(loudness); // Trigger the event for any speech above the quiet threshold
            aiHearing.OnLoudSpeechDetected(loudness); // Notify the AI
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