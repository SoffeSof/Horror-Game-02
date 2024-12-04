using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform player; // Drag the player object into this field in the Inspector
    private UnityEngine.AI.NavMeshAgent agent;
    public float respawnRadius = 10f; // Radius within which the monster respawns

    // Add a respawn delay
    public float respawnDelay = 1f; // Delay before the monster respawns

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object by tag
        RespawnMonster();
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position); // Continuously chase the player
        }
    }

    public void RespawnMonster()
    {
        int maxRetries = 10; // Max number of attempts to find a valid spot
        int attempts = 0;

        // Sample the NavMesh to find a valid walkable position
        UnityEngine.AI.NavMeshHit hit;
        float min = 30f;  // Minimum X value
        float max = 50f;  // Maximum X value

        do
        {
            // Generate a random X and Z position within the specified range
            float randomX = Random.Range(min, max); // Random X between minX and maxX
            float randomZ = Random.Range(min, max); // Random Z between minZ and maxZ

            // Keep Y the same as the original transform position, or modify if needed
            float randomY = transform.position.y; 

            // Generate random direction within the respawn radius
            Vector3 randomDirection = new Vector3(randomX, randomY, randomZ);
            randomDirection += transform.position;

            // Try to find a walkable position
            attempts++;


            if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, respawnRadius, UnityEngine.AI.NavMesh.AllAreas))
            {
                transform.position = hit.position;
                return; // Exit once a valid position is found
            }

            } while (attempts < maxRetries); // Retry if not found, until the max attempts

        // If we reach the retry limit without success, log a warning
        Debug.LogWarning("Could not find a valid walkable position after " + maxRetries + " attempts.");
    }
}
