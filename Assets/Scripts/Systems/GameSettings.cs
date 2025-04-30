using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    // Two preset sensitivity levels
    public float highSensitivity = 0.02f;  // More sensitive (quieter sounds will trigger)
    public float lowSensitivity = 0.01f;    // Less sensitive (only louder sounds will trigger)

    public bool isHighSensitivity = true;   // Default to high sensitivity

    public float CurrentSensitivity => isHighSensitivity ? highSensitivity : lowSensitivity;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}