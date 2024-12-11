using System.Collections;
using UnityEngine;

public class PlayerHeartbeat : MonoBehaviour
{
    public AudioSource heartbeatSource; // The AudioSource for the heartbeat
    public Transform playerTransform;

    private void Start()
    {
        if (heartbeatSource == null)
        {
            heartbeatSource = GetComponent<AudioSource>();
        }
        heartbeatSource.loop = true;
        heartbeatSource.spatialBlend = 1f; // Ensure 3D sound
        heartbeatSource.Play();
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector3 listenerPosition = Camera.main.transform.position; 
            float distanceToListener = Vector3.Distance(transform.position, listenerPosition);

            // Adjust volume based on distance 
            heartbeatSource.volume = Mathf.Clamp01(1f - (distanceToListener / heartbeatSource.maxDistance));

            // Ensure the local player doesn't hear their own heartbeat
            heartbeatSource.mute = distanceToListener < 0.1f;
        }
    }
}

