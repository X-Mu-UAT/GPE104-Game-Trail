using UnityEngine;

/// <summary>
/// Concrete implementation of the spaceship player pawn.
/// Handles physics-free vector math movement, teleportation, and boundary wrap overrides.
/// </summary>
public class SpaceShipPawn : Pawn
{
    [Header("Shooting Configurations")]
    [Tooltip("The bullet projectile prefab to spawn when firing.")]
    [SerializeField] private GameObject projectilePrefab;
    [Tooltip("The spawn point location at the tip of the ship.")]
    [SerializeField] private Transform firePoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for contact with any enemy object layers/tags
        // Note: Make sure your Enemy prefabs have the tag "Enemy" or an Asteroid tag set up
        if (collision.CompareTag("Enemy") || collision.CompareTag("Asteroid"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseLife();
                
                // Requirement: Reset the player ship back to the world origin upon death
                transform.position = Vector3.zero;
                
                // Reset rotation back to default forward facing position
                transform.rotation = Quaternion.identity;
            }
        }
    }

    /// <summary>
    /// Continuous local space movement using Vector math (Requirement).
    /// </summary>
    public override void MoveLocal(float forwardInput, float rotationInput, bool isTurbo)
    {
        // 1. Handle Turn Rotation (Z-Axis for 2D top-down)
        // rotationInput comes from Horizontal Axis (A/D or Left/Right Stick)
        float rotationAmount = rotationInput * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, -rotationAmount);

        // 2. Select designer specified speed based on Turbo state context
        float currentSpeed = isTurbo ? turboSpeed : moveSpeed;

        // 3. Vector Math Local Movement
        // In Unity 2D, transform.up represents the local "forward" direction of a top-down sprite
        Vector3 moveDirection = transform.up * forwardInput * currentSpeed * Time.deltaTime;
        transform.position += moveDirection;

        // 4. Wrap position through the screen boundary limits instantly
        if (GameManager.Instance != null)
        {
            transform.position = GameManager.Instance.WrapPosition(transform.position);
        }
    }

    /// <summary>
    /// Discrete world-space offset translation triggered once per press (Arrow Keys).
    /// </summary>
    public override void TeleportWorld(Vector3 translationOffset)
    {
        transform.position += translationOffset;
        
        // Enforce boundary looping wrap safety check
        if (GameManager.Instance != null)
        {
            transform.position = GameManager.Instance.WrapPosition(transform.position);
        }
    }

    /// <summary>
    /// Instantly teleports the spaceship to a random layout position inside screen boundaries (T Key).
    /// </summary>
    public override void TeleportRandom()
    {
        if (GameManager.Instance != null)
        {
            transform.position = GameManager.Instance.GetRandomBoundaryPosition();
        }
    }

    /// <summary>
    /// Instantiates a projectile moving in the direction the spaceship is currently facing.
    /// </summary>
    public override void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            if (GameManager.Instance != null && GameManager.Instance.playerShootClip != null)
            {
                GameManager.Instance.PlaySoundEffect(GameManager.Instance.playerShootClip, firePoint.position);
            }
        }
    }
}
