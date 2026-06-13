using UnityEngine;

/// <summary>
/// Handles the initial runtime population of the game world.
/// Distributes enemies randomly and calculates percentage-based variant scaling.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Layout Prefabs")]
    [Tooltip("The Large Asteroid prefab asset.")]
    [SerializeField] private GameObject largeAsteroidPrefab;

    [Tooltip("The UFO prefab asset.")]
    [SerializeField] private GameObject ufoPrefab;

    [Header("Spawn Density Controls (Exposed to Designers)")]
    [Tooltip("Total count of core enemy entities to generate when the match begins.")]
    [SerializeField] private int totalEnemiesToSpawn = 5;

    [Tooltip("Percentage probability (0 to 100) that a generated enemy will be a UFO instead of an Asteroid.")]
    [Range(0f, 100f)]
    [SerializeField] private float ufoSpawnChancePercentage = 30f;

    public EnemySpawner(float ufoSpawnChancePercentage)
    {
        this.ufoSpawnChancePercentage = ufoSpawnChancePercentage;
    }

    [Header("Safety Clear Zones")]
    [Tooltip("Minimum distance from the world origin (player spawn) where an enemy can appear, preventing instant deaths.")]
    [SerializeField] private float safeRadiusFromCenter = 3f;

    private void OnEnable()
    {
        // Execute the allocation grid sequence once the map initializes
        SpawnInitialEnemyWave();
    }

    /// <summary>
    /// Distributes randomized enemy variants across designer bounds.
    /// </summary>
    private void SpawnInitialEnemyWave()
    {
        // Safety check to ensure prefabs are assigned in the inspector
        if (largeAsteroidPrefab == null || ufoPrefab == null)
        {
            Debug.LogError("EnemySpawner: Prefabs are missing in the Inspector panel!");
            return;
        }

        // Change this line in your script:
        for (int i = totalEnemiesToSpawn - 1; i >= 0; i--)
        {
            // Your spawning logic goes inside here
        }

    } // end SpawnInitialEnemyWave

    /// <summary>
    /// Generates randomized coordinates within the GameManager boundaries while validating center safety rules.
    /// </summary>
    private Vector3 GetSafeRandomSpawnPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        int maxAttempts = 20;
        int attempts = 0;

        do
        {
            if (GameManager.Instance != null)
            {
                randomPosition = GameManager.Instance.GetRandomBoundaryPosition();
            }
            else
            {
                randomPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0f);
            }

            // Increment the counter safely
            attempts++;
        }
        // Ensure both checks use absolute comparisons (< or >), never a single '=' assignment!
        while (Vector3.Distance(randomPosition, Vector3.zero) < safeRadiusFromCenter && attempts < maxAttempts);

        return randomPosition;
    }
}