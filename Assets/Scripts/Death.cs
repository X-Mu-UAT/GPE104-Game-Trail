using System;
using UnityEngine;

/// <summary>
/// Handles explosion visual triggers and respawn positioning.
/// Updated to process polymorphic Enemy signatures per core architectural rules.
/// </summary>
public class Death : MonoBehaviour
{
    private Vector3 startPosition;

    public virtual void Awake()
    {
        // Cache our initial world orientation coordinates
        startPosition = transform.position;
    }

    /// <summary>
    /// Triggers destruction effects, checks ship context states, and manages system registrations.
    /// </summary>
    // FIXED: Changed 'Health healthRef' parameter over to the required structural 'Enemy enemyRef' type
    public virtual void Die(bool isPlayer, Enemy enemyRef)
    {
        Debug.Log("Die function called on: " + gameObject.name);

        // 1. Play the designer-assigned explosion effects
        ExplosionFX fxScript = GetComponent<ExplosionFX>();
        if (fxScript != null)
        {
            fxScript.PlayExplosion();
        }

        // 2. Clear out rigid physics momentum states
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // 3. Coordinate state tracking reports with the single GameManager instance
        if (GameManager.Instance != null)
        {
            if (isPlayer)
            {
                // Respawn behavior: teleport player back to origin points
                transform.position = startPosition;
            }
            else
            {
                // FIXED: Safely removes the obstacle track using the polymorphic Enemy type reference signature
                if (enemyRef != null)
                {
                    GameManager.Instance.UnregisterObstacle(enemyRef);
                }

                // Remove the targeted physical object instance from memory frames

                Destroy(gameObject);
            }
        }
    }

    internal void Die()
    {
        throw new NotImplementedException();
    }
}