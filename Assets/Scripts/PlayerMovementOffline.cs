using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovementOffline : MonoBehaviour
{
    public float speed = 5f;

    private PlayerControls controls; // Reference to the Input Actions class
    private Vector2 moveInput;

    void Awake()
    {
        // Initialize the Input Actions
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        // Enable the controls
        controls.Enable();

        // Assign the "Move" action to update moveInput
        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero; // Stop movement on release
    }

    void OnDisable()
    {
        // Disable the controls
        controls.Disable();
    }

    void Update()
    {
        
            // Apply movement
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime;
        transform.Translate(movement);
            
    }
}
