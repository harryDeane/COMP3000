using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Echolocation : MonoBehaviour
{
    [Header("Echolocation Settings")]
    public float pulseRadius = 10f;        // Radius of the echolocation pulse
    public float pulseDuration = 0.5f;    // Duration objects remain visible
    public float cooldown = 3f;           // Cooldown between pulses
    public LayerMask echoLayerMask;       // Layer mask for detecting objects

    [Header("Effects")]
    public Material outlineMaterial;      // Material for outline/glow effect

    private bool canPulse = true;         // Prevents spamming echolocation
    private PlayerControls controls;      // Reference to Input System actions

    void Awake()
    {
        // Initialize the Input System controls
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        // Enable Input System controls
        controls.Enable();

        // Bind the Echolocation action
        controls.Gameplay.Echolocation.performed += _ => TriggerEcholocation();
    }

    void OnDisable()
    {
        // Disable Input System controls
        controls.Disable();
    }

    void TriggerEcholocation()
    {
        // Start the pulse if not on cooldown
        if (canPulse)
        {
            StartCoroutine(EmitPulse());
        }
    }

    IEnumerator EmitPulse()
    {
        canPulse = false;

        // Detect objects within the pulse radius
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, pulseRadius, echoLayerMask);

        foreach (Collider hit in hitObjects)
        {
            // Apply visual effect to the hit object
            StartCoroutine(ShowEchoEffect(hit));
        }

        // Wait for cooldown
        yield return new WaitForSeconds(cooldown);
        canPulse = true;
    }

    IEnumerator ShowEchoEffect(Collider hitObject)
    {
        Renderer renderer = hitObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            // Save the original material
            Material originalMaterial = renderer.material;

            // Apply the outline material for visual effect
            renderer.material = outlineMaterial;

            // Wait for the pulse duration
            yield return new WaitForSeconds(pulseDuration);

            // Revert to the original material
            renderer.material = originalMaterial;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the echolocation pulse radius in the Scene view
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pulseRadius);
    }
}

