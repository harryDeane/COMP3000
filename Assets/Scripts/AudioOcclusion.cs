using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOcclusion : MonoBehaviour
{
    public Transform listener;
    public AudioSource audioSource; // The Audio Source to occlude
    public LayerMask occlusionLayer; // Layer for walls/obstacles
    public float maxVolume = 1.0f; // Maximum volume when not occluded
    public float occlusionFactor = 0.3f; // Volume reduction when occluded 
    public float occlusionCheckInterval = 0.1f; // How often to check for occlusion

    private float _timer;

    void Start()
    {
        if (listener == null)
        {
            Debug.LogError("Listener is not assigned in AudioOcclusion script.");
            enabled = false;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource is not assigned and not found on the GameObject.");
                enabled = false;
            }
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;

        
        if (_timer >= occlusionCheckInterval)
        {
            _timer = 0f;

            
            RaycastHit hit;
            if (Physics.Linecast(listener.position, audioSource.transform.position, out hit, occlusionLayer))
            {
               
                audioSource.volume = maxVolume * occlusionFactor;
            }
            else
            {
                
                audioSource.volume = maxVolume;
            }
        }
    }
}
