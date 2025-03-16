using Mirror;
using UnityEngine;
using UnityEngine.UI;


public class RoomPlayerUI : NetworkBehaviour
{
    [Header("UI Elements")]
    public Button readyButton;
    public Text readyButtonText; // Text for the ready button
    public Text playerNameText;  // Text for the player name

    private NetworkRoomPlayer roomPlayer;

    private void Start()
    {
        // Find the NetworkRoomPlayer component on this object
        roomPlayer = GetComponent<NetworkRoomPlayer>();

        if (isLocalPlayer)
        {
            // Set up the button for the local player
            readyButton.onClick.AddListener(ToggleReady);
            UpdateReadyButtonText();

            // Set the player name (you can replace this with a custom name input)
            playerNameText.text = "Player " + roomPlayer.index; // Example: "Player 1", "Player 2", etc.
        }
        else
        {
            // Disable the button for non-local players
            readyButton.interactable = false;

            // Set the player name for other players
            playerNameText.text = "Player " + roomPlayer.index;
        }
    }

    private void ToggleReady()
    {
        // Call the server to toggle the ready state
        if (isLocalPlayer)
        {
            roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
            UpdateReadyButtonText();
        }
    }

    private void UpdateReadyButtonText()
    {
        // Update the button text based on the ready state
        if (roomPlayer.readyToBegin)
        {
            readyButtonText.text = "Not Ready";
        }
        else
        {
            readyButtonText.text = "Ready";
        }
    }
}