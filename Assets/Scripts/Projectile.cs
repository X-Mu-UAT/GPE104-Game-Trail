using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float damageAmount = 10f;
    public float lifeSpan = 3f;

    void Start()
    {
        Destroy(gameObject, lifeSpan);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Safety Check: If the bullet hits the player who shot it, do nothing!
        if (other.CompareTag("Player"))
        {
            return;
        }

        Health targetHealth = other.GetComponent<Health>();

        if (targetHealth != null)
        {
            targetHealth.TakeDamage((int)damageAmount);
            Destroy(gameObject);
            return; // Exit early so it doesn't double-destroy
        }

        // Destroy the bullet if it hits a wall or something without a Health component
        Destroy(gameObject);
    } // Closes void OnTriggerEnter2D
} 