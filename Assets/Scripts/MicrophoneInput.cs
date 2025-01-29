using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    public float loudnessThreshold = 0.1f; // Threshold for detecting loud speech
    public AIHearing aiHearing; // Reference to the AIHearing script

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
        Debug.Log("Microphone Loudness: " + loudness); // Log the loudness for debugging

        if (loudness > loudnessThreshold)
        {
            Debug.Log("Player is speaking loudly: " + loudness);
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
