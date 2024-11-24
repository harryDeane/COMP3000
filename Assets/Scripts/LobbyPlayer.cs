using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LobbyPlayer : NetworkBehaviour
{
    [SyncVar] public bool isReady = false; // SyncVar to sync the ready state across all clients

    public void ToggleReady()
    {
        if (isLocalPlayer)
        {
            Debug.Log("Player is local. Proceeding to toggle ready state...");
            CmdSetReady(!isReady);
        }
        else
        {
            Debug.LogError("ToggleReady called, but this is not the local player!");
        }
    }


    [Command]
    void CmdSetReady(bool ready)
    {
        // Update the ready state on the server
        isReady = ready;
    }
}

