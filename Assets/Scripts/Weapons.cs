using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab; // Drag your Projectile prefab here in Inspector
    public Transform firePoint;          // Create an empty GameObject at the gun tip and drag it here

    // This is the function your UI Button will call
    public void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Spawn the projectile at the firePoint's position and rotation
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogWarning("Missing Projectile Prefab or Fire Point on Weapon script!");
        }
    }
}
