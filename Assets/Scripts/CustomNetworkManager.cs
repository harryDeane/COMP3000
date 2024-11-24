using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public string mainSceneName = "MainScene";

    // Called by the Start Game Button
    public void StartGame()
    {
        if (NetworkServer.active) // Only the server can start the game
        {
            // Check if all players are ready
            foreach (var conn in NetworkServer.connections.Values)
            {
                var player = conn.identity.GetComponent<LobbyPlayer>();
                if (player == null || !player.isReady)
                {
                    Debug.LogError("Not all players are ready!");
                    return;
                }
            }

            // All players are ready; transition to the main scene
            Debug.Log("All players ready. Starting game...");
            ServerChangeScene(mainSceneName);
        }
        else
        {
            Debug.LogError("StartGame called, but this is not the server!");
        }
    }

}
