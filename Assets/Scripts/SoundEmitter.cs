using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public float soundLoudness = 0.5f; // Loudness of the sound
    public float soundRange = 15f; // Range at which the sound can be heard

    private AIHearing[] aiPlayers; // Array of AI players that can hear the sound

    private void Start()
    {
        // Find all objects with the "Seeker" tag and get their AIHearing components
        FindSeekers();
    }

    // Call this method when the sound is played
    public void EmitSound()
    {
        Debug.Log("EmitSound called on " + gameObject.name);

        // Ensure the AI players array is up to date
        FindSeekers();

        foreach (var ai in aiPlayers)
        {
            if (ai != null)
            {
                ai.OnSoundDetected(transform.position, soundLoudness);
                Debug.Log("Sound is being emitted to AI: " + ai.gameObject.name);
            }
        }
    }

    // Helper method to find all objects with the "Seeker" tag and get their AIHearing components
    private void FindSeekers()
    {
        // Find all GameObjects with the "Seeker" tag
        GameObject[] seekerObjects = GameObject.FindGameObjectsWithTag("Seeker");

        // Create a list to store the AIHearing components
        List<AIHearing> aiHearingList = new List<AIHearing>();

        // Loop through each seeker object and get its AIHearing component
        foreach (var seeker in seekerObjects)
        {
            AIHearing aiHearing = seeker.GetComponent<AIHearing>();
            if (aiHearing != null)
            {
                aiHearingList.Add(aiHearing);
            }
        }

        // Convert the list to an array
        aiPlayers = aiHearingList.ToArray();
    }
}