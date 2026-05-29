using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float damageAmount = 10f; // Designer-adjustable in inspector
    public float lifeSpan = 3f;

    void Start()
    {
        Destroy(gameObject, lifeSpan); // Clean up bullet if it hits nothing
    }

    void Update()
    {
        // Move forward constantly in its local facing direction
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    // Trigger collision detection
    void OnTriggerEnter2D(Collider2D other)
    {
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damageAmount);
            Destroy(gameObject); // Destroy the bullet upon impact
        }
    }
}