using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{

    public void OnHostButtonClicked()
    {
        NetworkManager.singleton.StartHost(); // Start hosting the game
    }

    public void OnJoinButtonClicked()
    {
        NetworkManager.singleton.StartClient(); // Join as a client
    }

}