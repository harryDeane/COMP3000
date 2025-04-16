using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 

public class LoadingManager : MonoBehaviour
{
    [Header("Loading Screen UI References")]
    [SerializeField] private GameObject loadingScreenPrefab;

    [Header("Loading Settings")]
    [SerializeField] private float minimumLoadingTime = 3f;
    [SerializeField] private string[] loadingTips;

    // Runtime references
    private GameObject loadingScreenInstance;
    private Slider progressBar;
    private TextMeshProUGUI progressText; 
    private TextMeshProUGUI tipText;      

    private static LoadingManager _instance;
    private static bool _isLoading = false;

    public static LoadingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LoadingManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        if (loadingScreenPrefab == null)
        {
            Debug.LogError("Loading Screen Prefab is not assigned in the LoadingManager!");
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!_isLoading)
        {
            if (loadingScreenPrefab == null)
            {
                Debug.LogError("Cannot load scene: Loading Screen Prefab is missing. Loading directly.");
                SceneManager.LoadScene(sceneName);
                return;
            }

            StartCoroutine(LoadSceneAsync(sceneName));
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        _isLoading = true;

        CreateLoadingScreen();

        if (tipText != null && loadingTips != null && loadingTips.Length > 0)
        {
            tipText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float startTime = Time.time;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (progressText != null)
            {
                progressText.text = $"{Mathf.Round(progress * 100)}%";
            }

            yield return null;
        }

        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadingTime)
        {
            float remainingTime = minimumLoadingTime - elapsedTime;
            float startProgress = progressBar != null ? progressBar.value : 0.9f;

            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                float t = 1 - (remainingTime / (minimumLoadingTime - elapsedTime));
                float smoothProgress = Mathf.Lerp(startProgress, 1.0f, t);

                if (progressBar != null)
                {
                    progressBar.value = smoothProgress;
                }

                if (progressText != null)
                {
                    progressText.text = $"{Mathf.Round(smoothProgress * 100)}%";
                }

                yield return null;
            }
        }

        if (progressBar != null)
        {
            progressBar.value = 1.0f;
        }

        if (progressText != null)
        {
            progressText.text = "100%";
        }

        operation.allowSceneActivation = true;

        yield return new WaitForSeconds(0.5f);

        DestroyLoadingScreen();

        _isLoading = false;
    }

    private void CreateLoadingScreen()
    {
        if (loadingScreenInstance == null)
        {
            loadingScreenInstance = Instantiate(loadingScreenPrefab);
            DontDestroyOnLoad(loadingScreenInstance);

            Canvas canvas = loadingScreenInstance.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = 999;
                canvas.worldCamera = Camera.main;
            }

            progressBar = loadingScreenInstance.GetComponentInChildren<Slider>();

            
            TextMeshProUGUI[] texts = loadingScreenInstance.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                if (text.gameObject.name.Contains("Progress"))
                {
                    progressText = text;
                }
                else if (text.gameObject.name.Contains("Tip"))
                {
                    tipText = text;
                }
            }
           
        }
        else
        {
            loadingScreenInstance.SetActive(true);
        }
    }

    private void DestroyLoadingScreen()
    {
        if (loadingScreenInstance != null)
        {
            Destroy(loadingScreenInstance);
            loadingScreenInstance = null;
            progressBar = null;
            progressText = null;
            tipText = null;
        }
    }
}
