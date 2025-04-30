using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityToggle : MonoBehaviour
{
    public Toggle sensitivityToggle;

    void Start()
    {
        // Initialize with current settings
        sensitivityToggle.isOn = GameSettings.Instance.isHighSensitivity;

        // Add listener
        sensitivityToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isHighSensitivity)
    {
        GameSettings.Instance.isHighSensitivity = isHighSensitivity;
    }

   
}
