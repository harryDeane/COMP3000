using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private LeverProgress[] levers; // Reference to all lever scripts
    [SerializeField] private GameObject actionObject; // Object to activate or action to trigger

    private void Update()
    {
        // Check if all levers are complete
        if (AllLeversComplete())
        {
            TriggerAction();
        }
    }

    private bool AllLeversComplete()
    {
        foreach (var lever in levers)
        {
            if (!lever.IsComplete) return false; // Custom property `IsComplete` in `LeverProgress`
        }
        return true;
    }

    private void TriggerAction()
    {
        Debug.Log("All levers complete!");
        actionObject.SetActive(true); // Example action
    }
}

