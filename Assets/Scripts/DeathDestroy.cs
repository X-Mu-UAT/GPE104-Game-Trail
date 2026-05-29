using UnityEngine;

public class DeathDestroy : Death
{
    public GameObject explosionPrefab;

    public override void Die()
    {
        // FIXED: Added a check to ensure you don't accidentally clone yourself
        if (explosionPrefab != null && explosionPrefab != gameObject)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
        else if (explosionPrefab == gameObject)
        {
            Debug.LogWarning("Warning: You assigned the Spaceship as its own Explosion Prefab!");
        }

        // Run the teleport and health reset
        base.Die();
    }
}
