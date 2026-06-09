using UnityEngine;

/// <summary>
/// Projectile behavior script. Handles linear forward vector translation,
/// designer-facing damage parameters, screen-warping, and enemy contact processing.
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [Tooltip("How fast the bullet travels forward.")]
    [SerializeField] private float speed = 10f;

    [Tooltip("The raw damage value applied to hit enemies.")]
    [SerializeField] private float damageAmount = 25f;

    [Tooltip("Maximum life duration in seconds before self-destructing.")]
    [SerializeField] private float lifeSpan = 3f;

    private void Start()
    {
        // Automatically self-destructs after its designer-specified life-span ticks away
        Destroy(gameObject, lifeSpan);
    }

    private void Update()
    {
        // Moves forward along its own local up vector (Y-axis matching top-down space)
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);

        // Requirement: Screen warping loop (bullets loop around boundary area too!)
        if (GameManager.Instance != null)
        {
            transform.position = GameManager.Instance.WrapPosition(transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Safety Check: If the bullet hits the player who shot it, do nothing!
        if (other.CompareTag("Player"))
        {
            return;
        }

        // 2. Safety Check: If you hit boundary triggers, ignore them so the bullet can warp instead of dying
        if (other.CompareTag("Boundary"))
        {
            return;
        }

        // UPDATED: Query the polymorphic abstract Enemy component instead of the old legacy Health file
        Enemy targetEnemy = other.GetComponent<Enemy>();
        if (targetEnemy != null)
        {
            // Apply damage converted safely to an integer
            targetEnemy.TakeDamage((int)damageAmount);
            
            // Instantly remove bullet from the world after delivery
            Destroy(gameObject);
            return; 
        }

        // Destroy the bullet if it hits an environment block or unhandled physical entity
        Destroy(gameObject);
    }
}
