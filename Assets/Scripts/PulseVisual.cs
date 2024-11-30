using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseVisual : MonoBehaviour
{
    public float expansionSpeed = 5f; // Speed of the ring expansion
    public float maxScale = 10f;      // Maximum size before the ring disappears
    public float fadeDuration = 1f;  // Time to fade out the ring

    private Material material;        // Reference to the ring's material
    private float initialAlpha;
    private float initialYScale;      // Stores the original Y scale

    void Start()
    {
        // Get the material and store its initial alpha value
        material = GetComponent<Renderer>().material;
        initialAlpha = material.color.a;

        // Store the initial Y scale to keep it constant
        initialYScale = transform.localScale.y;
    }

    void Update()
    {
        // Expand the X and Z axes only
        transform.localScale += new Vector3(expansionSpeed * Time.deltaTime, 0, expansionSpeed * Time.deltaTime);

        // Keep the Y scale constant
        transform.localScale = new Vector3(transform.localScale.x, initialYScale, transform.localScale.z);

        // Fade out the ring over time
        Color color = material.color;
        color.a = Mathf.Lerp(initialAlpha, 0, transform.localScale.x / maxScale);
        material.color = color;

        // Destroy the ring when it reaches the maximum scale
        if (transform.localScale.x >= maxScale)
        {
            Destroy(gameObject);
        }
    }
}
