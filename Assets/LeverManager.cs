using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private LeverProgress[] levers; // Reference to all lever scripts
    [SerializeField] private GameObject actionObject; // Object to activate or action to trigger
    [SerializeField] private GameObject actionObject2; // Object to activate or action to trigger

    public Animator exitAnimator;
    public Animator exitAnimator2;

    // Name of the animation state to play
    [SerializeField] private string animationStateName = "Open";

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
        actionObject.SetActive(true);
        actionObject2.SetActive(true);

        // Play the animation on both animators
        if (exitAnimator != null)
        {
            exitAnimator.Play(animationStateName);
        }

        if (exitAnimator2 != null)
        {
            exitAnimator2.Play(animationStateName);
        }

    }
}

