using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartLevel1()
    {
        LoadingManager.Instance.LoadScene("Main");
    }

    public void StartLevel2()
    {
        LoadingManager.Instance.LoadScene("Level2");
    }

    public void StartMultiplayer()
    {
        SceneManager.LoadSceneAsync("LobbyScene");
    }

    public void StartSingleplayer()
    {
        LoadingManager.Instance.LoadScene("MainMenu2");
    }

}
