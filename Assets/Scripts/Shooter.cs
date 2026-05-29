using UnityEngine;

public abstract class Shooter : MonoBehaviour
{
    [Header("Shooter Base Settings")]
    public GameObject projectilePrefab;
    public float fireRate = 0.5f;

    // Abstract method: must be implemented by child classes (Polymorphism)
    public abstract void Shoot();
}