using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public float soundLoudness = 0.5f; // Loudness of the sound
    public float soundRange = 15f; // Range at which the sound can be heard
    public AIHearing[] aiPlayers; // Array of AI players that can hear the sound

    // Call this method when the sound is played
    public void EmitSound()
    {
        foreach (var ai in aiPlayers)
        {
            if (ai != null)
            {
                ai.OnSoundDetected(transform.position, soundLoudness);
            }
        }
    }
}
