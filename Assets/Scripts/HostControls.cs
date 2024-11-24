using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HostControls : MonoBehaviour
{
    public Button startButton;

    void Update()
    {
        if (!NetworkServer.active)
        {
            startButton.interactable = false;
            return;
        }

        // Check if all players are ready
        bool allReady = true;
        foreach (var conn in NetworkServer.connections.Values)
        {
            var player = conn.identity.GetComponent<LobbyPlayer>();
            if (player == null || !player.isReady)
            {
                allReady = false;
                break;
            }
        }

        startButton.interactable = allReady;
    }
}

