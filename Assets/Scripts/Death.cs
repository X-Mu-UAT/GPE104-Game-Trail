using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Death : MonoBehaviour
{
    private Vector3 startPosition;

    // Changed to Awake to guarantee the starting position is saved instantly
    public virtual void Awake()
    {
        startPosition = transform.position;
    }

    public virtual void Die()
    {
        Debug.Log("Die function called on: " + gameObject.name);

        // 1. Teleport back to the starting point instead of disappearing 
        transform.position = startPosition;

        // 2. Reset the physics velocity so it stops moving/falling 
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // 3. Reset the Health script back to full 
        Health targetHealth = GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.ResetHealth();
        }
    } // Fixed missing closing bracket for Die()
} // Fixed missing closing bracket for the class

