using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRPlayerAnimation : MonoBehaviour
{
    public Animator animator; // Reference to Animator component
    public InputActionReference moveInput; // Reference to the movement input action

    private void Update()
    {
        Vector2 movement = moveInput.action.ReadValue<Vector2>(); // Get movement input
        float moveMagnitude = movement.magnitude; // Get movement strength

        animator.SetFloat("MoveSpeed", moveMagnitude); // Set animation parameter
    }
}
