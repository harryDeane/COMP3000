using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoInteractable : MonoBehaviour
{
    public Material highlightMaterial; // Assign a glowing or emissive material in the Inspector
    private Material originalMaterial; // Store the original material
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
        }
    }

    public void HighlightSurface(Vector3 hitPoint, Vector3 hitNormal, float duration)
    {
        GameObject lightObject = new GameObject("EcholocationLight");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = 2f;
        light.intensity = 3f;
        light.color = Color.cyan;

        // Position and orient the light based on the collision
        lightObject.transform.position = hitPoint + hitNormal * 0.1f; // Offset slightly to avoid z-fighting
        Destroy(lightObject, duration); // Automatically clean up after the duration
    }


    private void RevertMaterial()
    {
        if (renderer != null)
        {
            renderer.material = originalMaterial;
        }
    }
}
