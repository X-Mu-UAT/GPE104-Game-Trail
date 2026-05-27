using UnityEngine;

public class DeathDestroy : Death
{
    public GameObject explosionPrefab;
    // This is a simple implementation of the Death class that destroys the GameObject when it dies. You can use this for any GameObject that you want to be destroyed when it dies, like enemies or NPCs. You can also override the Die() method in child classes to add specific logic for each type of death, like playing a death animation before destroying the GameObject.
    public override void Die()
    {
        
        if (explosionPrefab != null)
        {
            // Instantiate the explosion effect at the position of the GameObject
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // Destroy the GameObject or clear it from the game world
            Destroy(gameObject);
        }
    }
}
