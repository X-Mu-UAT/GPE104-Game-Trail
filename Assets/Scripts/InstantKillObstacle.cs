using UnityEngine;

public class InstantKillObstacle : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Health playerHealth = collision.gameObject.GetComponent<Health>();
        
        // If it has a health component and it is marked as the player, instantly kill it
        if (playerHealth != null && playerHealth.isPlayer)
        {
            playerHealth.Die();
        }
    }
}