using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnTime = 4f;

    [Header("Spawn Settings")]
    public float spawnRadius = 12f; // Distance from the player to spawn meteors
    public string playerTag = "Player"; // Make sure your player object has this tag!

    private float timer = 0f;
    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindWithTag(playerTag);
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("<color=green>SUCCESS:</color> Found the player! Meteors will spawn around " + playerTransform.position);
        }
        else
        {
            Debug.LogError("<color=red>ERROR:</color> Could not find your Player! Make sure your player object has the tag 'Player' in the Inspector.");
        }
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTime)
        {
            int currentMeteors = GameObject.FindGameObjectsWithTag("Asteroid").Length;

            if (currentMeteors < 5)
            {
                SpawnMeteor();
            }

            timer = 0f;
        }
    }

    void SpawnMeteor()
    {
        Vector3 spawnPosition = transform.position;

        // If the player exists, calculate a random position around them
        if (playerTransform != null)
        {
            // Generates a random point on a circle edge, then scales it by spawnRadius
            Vector2 randomCirclePoint = Random.insideUnitCircle.normalized * spawnRadius;

            // Set the spawn position relative to the player's current location
            spawnPosition = new Vector3(
                playerTransform.position.x + randomCirclePoint.x,
                playerTransform.position.y + randomCirclePoint.y,
                0f // Set to playerTransform.position.z if your game is 3D
            );
        }
        else
        {
            // Fallback: If player isn't found, try to find them again
            GameObject player = GameObject.FindWithTag(playerTag);
            if (player != null) playerTransform = player.transform;
        }

        // Spawn the meteor at the calculated position
        Instantiate(meteorPrefab, spawnPosition, transform.rotation);
    }
}
