using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GeneratorSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // The GameObject to spawn and move towards the player
    public Transform[] generatorPositions; // Array of generator positions
    public Transform player; // Reference to the player's transform
    public float spawnInterval = 2f; // Time interval between spawns

    private List<Transform> activeGenerators = new List<Transform>(); // List of active (non-complete) generators

    private void Start()
    {
        if (generatorPositions.Length == 0 || player == null || objectToSpawn == null)
        {
            Debug.LogError("Please assign all required fields in the Inspector.");
            return;
        }

        // Initialize the list of active generators
        foreach (Transform generator in generatorPositions)
        {
            activeGenerators.Add(generator);
        }

        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            // Remove completed generators from the active list
            RemoveCompletedGenerators();

            // If there are no active generators, stop spawning
            if (activeGenerators.Count == 0)
            {
                Debug.Log("All generators are complete. Stopping spawner.");
                yield break; // Exit the coroutine
            }

            // Find the closest active generator to the player
            Transform closestGenerator = GetClosestGenerator();

            if (closestGenerator != null)
            {
                // Spawn the object at the closest active generator's position
                GameObject spawnedObject = Instantiate(objectToSpawn, closestGenerator.position, Quaternion.identity);
                NavMeshAgent agent = spawnedObject.GetComponent<NavMeshAgent>();

                if (agent != null)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    Debug.LogError("Spawned object does not have a NavMeshAgent component.");
                }

                // Attach a script to handle collision with the player
                spawnedObject.AddComponent<ObjectCollisionHandler>().Initialize(player);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Transform GetClosestGenerator()
    {
        Transform closestGenerator = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform generator in activeGenerators)
        {
            float distanceToPlayer = Vector3.Distance(generator.position, player.position);

            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestGenerator = generator;
            }
        }

        return closestGenerator;
    }

    private void RemoveCompletedGenerators()
    {
        // Create a list to store generators that need to be removed
        List<Transform> generatorsToRemove = new List<Transform>();

        foreach (Transform generator in activeGenerators)
        {
            // Check if the generator is complete
            LeverProgress leverProgress = generator.GetComponent<LeverProgress>();

            if (leverProgress != null && leverProgress.IsComplete)
            {
                generatorsToRemove.Add(generator);
            }
        }

        // Remove completed generators from the active list
        foreach (Transform generator in generatorsToRemove)
        {
            activeGenerators.Remove(generator);
            Debug.Log($"Generator {generator.name} is complete and removed from active generators.");
        }
    }
}