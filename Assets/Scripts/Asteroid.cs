using UnityEngine;

/// <summary>
/// Asteroid variation class that inherits from my existing Enemy base file.
/// Handles the random launch trajectories and recursive multi-stage splitting rules.
/// </summary>
public class Asteroid : Enemy
{
    // Mapping system to classify structural sizes inside the editor
    public enum AsteroidSize { Large, Medium, Small }

    [Header("Asteroid Variant Architecture")]
    [Tooltip("The designated size classification of this specific prefab file asset.")]
    [SerializeField] private AsteroidSize sizeCategory = AsteroidSize.Large;

    [Tooltip("The smaller nested asteroid prefab to clone when this entity falls to zero health.")]
    [SerializeField] private GameObject lowerTierFragmentPrefab;

    [Tooltip("The designer-designated number of fragment units to spawn upon zero health.")]
    [Range(0, 5)]
    [SerializeField] private int fragmentSpawnCount = 2;

    private Vector3 movementDirection;

    protected override void Start()
    {
        // 1. MUST EXECUTE: Runs your existing Enemy.cs setup (sets max health, shows sliders, registers with GM)
        base.Start();

        // 2. Requirement: Choose an X,Y direction completely at random when entering the game world
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        movementDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f).normalized;
    }

    protected override void Update()
    {
        // 1. Move slowly over time using pure Vector math transformations based on your inherited speed float
        transform.position += movementDirection * speed * Time.deltaTime;

        // 2. MUST EXECUTE: Runs your existing Enemy.cs loop (keeps health bar straight, handles screen warp maps)
        base.Update();
    }

    /// <summary>
    /// Overrides the standard baseline destruction function to handle fragment spawning.
    /// </summary>
    protected override void Die()
    {
        // Requirement: Large spawns Medium, Medium spawns Small, Small dies instantly without spawning anything
        if (sizeCategory != AsteroidSize.Small)
        {
            SpawnBreakdownFragments();
        }

        // Run your existing baseline Enemy.cs Die() method to update score, play sounds, and clear memory
        base.Die();
    }

    /// <summary>
    /// Spawns the smaller fragment variations right at the destruction coordinates.
    /// </summary>
    private void SpawnBreakdownFragments()
    {
        if (lowerTierFragmentPrefab == null)
        {
            Debug.LogWarning($"[Asteroid] {gameObject.name} is missing its lower-tier fragment prefab reference assignment!");
            return;
        }

        for (int i = 0; i < fragmentSpawnCount; i++)
        {
            GameObject fragmentObj = Instantiate(lowerTierFragmentPrefab, transform.position, Quaternion.identity) as GameObject;
            if (fragmentObj == null) continue;

            // Slight offset to avoid exact overlap
            fragmentObj.transform.position = transform.position + (Vector3)(Random.insideUnitCircle * 0.1f);

            Enemy fragmentEnemy = fragmentObj.GetComponent<Enemy>();
            if (fragmentEnemy != null && GameManager.Instance != null)
            {
                GameManager.Instance.RegisterObstacle(fragmentEnemy);
            }
        }
    }
}
