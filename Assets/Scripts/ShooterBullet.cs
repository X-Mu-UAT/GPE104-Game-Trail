using UnityEngine;

public class ShooterBullet : Shooter
{
    // Override the base class method (Polymorphism)
    public override void Shoot()
    {
        if (projectilePrefab == null) return;

        // Calculate position 1 unit in front of the ship (using 2D transform.up based on your previous rotation script)
        Vector3 spawnPosition = transform.position + (transform.up * 1f);

        // Spawn the projectile
        GameObject bullet = Instantiate(projectilePrefab, spawnPosition, transform.rotation);
    }
}