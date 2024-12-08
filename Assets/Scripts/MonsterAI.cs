using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public Transform player; // Drag the player object into this field in the Inspector
    private UnityEngine.AI.NavMeshAgent agent; // The NavMeshAgent component used for pathfinding
    public float respawnRadius = 30f; // Radius within which the monster respawns
    private float monsterSanityDrainRate = 12f; // Sanity drain rate when the monster is close to the player
    private float normalSanityDrainRate = 0.5f; // Default sanity drain rate when the monster is not close to the player

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); // Get the NavMeshAgent component attached to the monster
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player object by its tag
        RespawnMonster(); // Respawn the monster to a valid position
    }

    void Update()
    {
        if (player != null) // If the player exists, set the monster's destination to the player's position
        {
            agent.SetDestination(player.position); // Continuously chase the player
        }
    }

    public void RespawnMonster()
    {
        int maxRetries = 10; // Max number of attempts to find a valid spot
        int attempts = 0; // Number of attempts made so far

        UnityEngine.AI.NavMeshHit hit; // Sample the NavMesh to find a valid walkable position
        float min = -50f;  // Minimum X value for respawn area
        float max = 50f;  // Maximum X value for respawn area

        do // Loop until a valid position is found or maxRetries is reached
        {
            // Decide whether to fix x or z to 30 or -30
            float randomX = (Random.value > 0.5f) ? min : max; // Randomly pick 30 or -30 for x
            float randomZ = Random.Range(min, max); // Randomize z within a range

            // Keep Y the same as the original transform position, or modify if needed
            float randomY = transform.position.y; 

            Vector3 randomDirection = new Vector3(randomX, randomY, randomZ);  // Create a random direction for respawn position within the defined area
            randomDirection += transform.position; // Offset it by the current monster position

            attempts++; // Increment the retry counter
            if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, respawnRadius, UnityEngine.AI.NavMesh.AllAreas))  // If a valid position is found, update the monster's position and return
            {
                transform.position = hit.position; // Set the monster's position to the valid position found
                return; // Exit once a valid position is found
            }

        } while (attempts < maxRetries); // Retry if not found, until the max attempts are reached

        Debug.LogWarning("Could not find a valid walkable position after " + maxRetries + " attempts."); // Log a warning if no valid position was found after maxRetries
    }

    private void OnTriggerEnter(Collider other) // Trigger when the monster collides with another collider
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to the player
        {
            Debug.Log("Monster collided with player"); 
            HUDController.Instance.sanityDrainRate = monsterSanityDrainRate; // Start draining faster sanity when the monster is near the player
        }
    }

    private void OnTriggerExit(Collider other) // Trigger when the monster exits the player's collider
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to the player
        {
            HUDController.Instance.sanityDrainRate = normalSanityDrainRate;// Stop draining sanity faster when the monster exits the player's collider
        }
    }
}
