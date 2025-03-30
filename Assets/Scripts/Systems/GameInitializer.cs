using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        // Wait for a frame to ensure everything is initialized
        Invoke("LoadMainMenu", 0.1f);
    }

    private void LoadMainMenu()
    {
        // Use the LoadingManager to load the main menu
        if (LoadingManager.Instance != null)
        {
            LoadingManager.Instance.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("LoadingManager instance not found! Make sure it's in the scene.");
        }
    }
}
