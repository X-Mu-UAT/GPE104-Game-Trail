using UnityEngine;

public class InstantKillObstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Health targetHealth = other.GetComponent<Health>();

        if (targetHealth != null)
        {
            targetHealth.TakeDamage(999999);
        }
    }
}