using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeDoor : MonoBehaviour
{
    private void Start()
    {
        // Ensure the door is initially inactive
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has touched the door
        if (other.CompareTag("Hider"))
        {
            // Load the GameOverScene
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
