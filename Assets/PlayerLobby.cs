using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerLobby : NetworkBehaviour
{
    [SyncVar]
    public bool isReady = false;

    public void SetReady(bool ready)
    {
        isReady = ready;
    }
}