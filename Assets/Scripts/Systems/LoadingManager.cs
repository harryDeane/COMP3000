using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Header("Loading Screen UI References")]
    [SerializeField] private GameObject loadingScreenPrefab;

    [Header("Loading Settings")]
    [SerializeField] private float minimumLoadingTime = 1.5f;
    [SerializeField] private string[] loadingTips;

    // Runtime references
    private GameObject loadingScreenInstance;
    private Slider progressBar;
    private Text progressText;
    private Text tipText;

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

        // Check for required prefab
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

        // Create the loading screen UI
        CreateLoadingScreen();

        // Display a random tip
        if (tipText != null && loadingTips != null && loadingTips.Length > 0)
        {
            tipText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }

        // Start the async load
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float startTime = Time.time;

        // Wait until the load is mostly complete
        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Update UI if available
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

        // Ensure minimum loading time
        float elapsedTime = Time.time - startTime;
        if (elapsedTime < minimumLoadingTime)
        {
            float remainingTime = minimumLoadingTime - elapsedTime;
            float startProgress = progressBar != null ? progressBar.value : 0.9f;

            // Animate to 100% during the remaining time
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

        // Complete loading
        if (progressBar != null)
        {
            progressBar.value = 1.0f;
        }

        if (progressText != null)
        {
            progressText.text = "100%";
        }

        // Allow the scene to activate
        operation.allowSceneActivation = true;

        // Wait for the scene to fully activate
        yield return new WaitForSeconds(0.5f);

        // Destroy the loading screen
        DestroyLoadingScreen();

        _isLoading = false;
    }

    private void CreateLoadingScreen()
    {
        // Only create if it doesn't exist
        if (loadingScreenInstance == null)
        {
            // Instantiate the loading screen prefab
            loadingScreenInstance = Instantiate(loadingScreenPrefab);

            // Make sure it survives scene changes
            DontDestroyOnLoad(loadingScreenInstance);

            // Ensure the canvas renders on top
            Canvas canvas = loadingScreenInstance.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = 999; // Highest priority
                canvas.worldCamera = Camera.main; // For VR, you might need to set this
            }

            // Find the references within the instantiated prefab
            progressBar = loadingScreenInstance.GetComponentInChildren<Slider>();

            // Find Text components - can be extended for TextMeshPro if needed
            Text[] texts = loadingScreenInstance.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
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
            // Make sure it's active
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