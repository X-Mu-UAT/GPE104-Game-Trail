using UnityEngine;

public class ExplosionFX : MonoBehaviour
{
    [Header("Visual Effects")]
    [Tooltip("The explosion particle prefab from your Space Shooter asset pack.")]
    public GameObject explosionPrefab;

    // This public method can be called from ANY script right before an object dies
    public void PlayExplosion()
    {
        if (explosionPrefab != null)
        {
            // Spawn the explosion at the exact location of this object
            GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, transform.rotation);

            // Automatically clean up the explosion clone after 2 seconds so it doesn't clutter your hierarchy
            Destroy(explosionInstance, 2.0f);
        }
    }
}