using System;
using UnityEngine;

// CHANGED: Removed 'abstract' so this script can be attached directly to GameObjects in Unity 
public class Death : MonoBehaviour
{
    private Vector3 startPosition;

    public virtual void Awake()
    {
        startPosition = transform.position;
    }

    // UPDATED: Added parameters so the function knows if the player or an obstacle died 
    public virtual void Die(bool isPlayer, Health healthRef)
    {
        Debug.Log("Die function called on: " + gameObject.name);

        // 🌟 ADDED: Look for the ExplosionFX component on this object and play it right as they die
        ExplosionFX fxScript = GetComponent<ExplosionFX>();
        if (fxScript != null)
        {
            fxScript.PlayExplosion();
        }

        // 1. Reset physical movement states 
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Note: If using older Unity versions, change 'linearVelocity' to 'velocity' 
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // 2. Report the death to the GameManager Singleton 
        if (GameManager.Instance != null)
        {
            if (isPlayer)
            {
                // Teleport player back to start and reset health as a respawn mechanic 
                transform.position = startPosition;
                if (healthRef != null)
                {
                    healthRef.ResetHealth();
                }
            }
            else
            {
                // Tells GameManager an obstacle was destroyed to update the list count 
                GameManager.Instance.UnregisterObstacle(healthRef);

                // Completely remove the obstacle from the game world 
                Destroy(gameObject);
            }
        }
    }

    internal void Die()
    {
        throw new NotImplementedException();
    }
}