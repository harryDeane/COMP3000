using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectCollisionHandler : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private float updateInterval = 0.5f; // Update destination every 0.5 seconds

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the spawned object.");
            return;
        }

        // Start the coroutine to update the destination
        StartCoroutine(UpdateDestination());
    }

    private IEnumerator UpdateDestination()
    {
        while (true)
        {
            if (player != null && agent != null)
            {
                agent.SetDestination(player.position);
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            Debug.Log("Collided with player, destroying object.");
            Destroy(gameObject);
        }
    }
}