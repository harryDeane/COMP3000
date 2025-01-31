using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartLevel1()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void StartLevel2()
    {
        SceneManager.LoadSceneAsync("Level2");
    }
}
