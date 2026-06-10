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
        if (other.CompareTag("Player") || other.CompareTag("Boundary"))
        {
            return;
        }

        // Search the object and any parent elements for the structural Enemy script type
        Enemy targetEnemy = other.GetComponentInParent<Enemy>();
        if (targetEnemy != null)
        {
            // Apply damage parameter
            targetEnemy.TakeDamage((int)damageAmount);

            // Destroy bullet instance
            Destroy(gameObject);
            return;
        }

        // Destroy the bullet if it hits a decorative physical barrier
        Destroy(gameObject);
    }
}

