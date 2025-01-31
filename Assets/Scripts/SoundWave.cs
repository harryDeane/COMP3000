using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public float expansionSpeed = 5f;   // Speed of the wave's expansion
    public float maxScale = 10f;        // Maximum scale for the wave
    public float highlightDuration = 1f; // Duration to highlight objects
    public LayerMask obstacleLayer;     // Layer mask to detect obstacles
    public LayerMask terrainLayer;      // Layer mask for the terrain
    public Color highlightColor = Color.yellow; // Color for the terrain highlight
    public GameObject decalPrefab;

    private bool shouldExpand = true;   // Determines whether the wave should keep expanding
    private SphereCollider sphereCollider;

    void Start()
    {
        // Ensure the collider matches the scale of the wave
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.radius = transform.localScale.x / 2f;
        }
    }

    void Update()
    {
        if (shouldExpand)
        {
            // Expand the wave
            transform.localScale += Vector3.one * expansionSpeed * Time.deltaTime;

            // Update the collider radius to match the wave scale
            if (sphereCollider != null)
            {
                sphereCollider.radius = transform.localScale.x / 2f;
            }

            // Destroy the wave if it reaches maximum scale
            if (transform.localScale.x >= maxScale)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is on the terrain layer
        if (((1 << other.gameObject.layer) & terrainLayer) != 0)
        {
            HighlightTerrain(other);
        }
        else
        {
            EchoInteractable interactable = other.GetComponent<EchoInteractable>();
            if (interactable != null)
            {
                // Line-of-sight check
                Vector3 directionToTarget = other.transform.position - transform.position;
                if (Physics.Raycast(transform.position, directionToTarget.normalized, out RaycastHit hit, directionToTarget.magnitude, obstacleLayer))
                {
                    if (hit.collider == other)
                    {
                        // Highlight the specific surface hit
                        interactable.HighlightSurface(hit.point, hit.normal, highlightDuration);
                        Debug.Log($"Illuminated {other.name} at {hit.point}");
                    }
                    else
                    {
                        Debug.Log($"{other.name} is blocked by {hit.collider.name}");
                    }
                }
            }
        }
    }

    void HighlightTerrain(Collider terrainCollider)
    {
        // Get the Terrain component
        Terrain terrain = terrainCollider.GetComponent<Terrain>();
        if (terrain != null)
        {
            // Get the terrain material
            Material terrainMaterial = terrain.materialTemplate;

            if (terrainMaterial != null)
            {
                // Enable emission and set the highlight color
               // terrainMaterial.EnableKeyword("_EMISSION"); 
               // terrainMaterial.SetColor("_EmissionColor", highlightColor);

                // Start a coroutine to reset the emission after the highlight duration
                StartCoroutine(ResetTerrainEmission(terrainMaterial));

                Debug.Log("Highlighting terrain at: " + terrainCollider.transform.position);
            }
            else
            {
                Debug.Log("Terrain material is null.");
            }
        }
        else
        {
            Debug.Log("Terrain component not found.");
        }
    }

    IEnumerator ResetTerrainEmission(Material terrainMaterial)
    {
        // Wait for the highlight duration
        yield return new WaitForSeconds(highlightDuration);

        // Reset the emission color to black (no emission)
        terrainMaterial.SetColor("_EmissionColor", Color.black);

        // Mark the material as dirty so Unity updates it
        //UnityEditor.EditorUtility.SetDirty(terrainMaterial);
    }
}


