using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ReadyButtonAssigner : NetworkBehaviour
{
    private Button readyButton;

    void Start()
    {
        // Only assign the button for the local player
        if (isLocalPlayer)
        {
            // Find the Ready Button dynamically in the scene
            readyButton = GameObject.FindWithTag("ReadyButton").GetComponent<Button>();

            if (readyButton != null)
            {
                // Dynamically assign the OnClick listener to call ToggleReady()
                readyButton.onClick.AddListener(() => GetComponent<LobbyPlayer>().ToggleReady());
            }
            else
            {
                Debug.LogError("Ready Button not found in the scene! Make sure it has the correct tag.");
            }
        }
    }
}


