using UnityEngine;

/// <summary>
/// Abstract Pawn base class handling the structural division of control inputs.
/// Exposes required design values to inspectors.
/// </summary>
public abstract class Pawn: MonoBehaviour
{
    [Header("Exposed Movement Speeds (Designer Tweakable)")]
    [Tooltip("Standard continuous movement speed.")]
    [SerializeField] protected float moveSpeed = 5f;

    [Tooltip("Rotation speed in degrees per second.")]
    [SerializeField] protected float turnSpeed = 180f;

    [Tooltip("Turbo movement speed activated by holding down the Shift key.")]
    [SerializeField] protected float turboSpeed = 10f;

    // Public getters to expose these speeds to designers/other scripts without making them public variables
    public float MoveSpeed => moveSpeed;
    public float TurnSpeed => turnSpeed;
    public float TurboSpeed => turboSpeed;

    /// <summary>
    /// Handles continuous local translation and rotation calculations.
    /// </summary>
    public abstract void MoveLocal(float forwardInput, float rotationInput, bool isTurbo);

    /// <summary>
    /// Performs a discrete, immediate shift in world-space coordinates (Arrow Keys).
    /// </summary>
    public abstract void TeleportWorld(Vector3 translationOffset);

    /// <summary>
    /// Teleports the pawn to a random location inside the active game boundary limits (T Key).
    /// </summary>
    public abstract void TeleportRandom();

    /// <summary>
    /// Instantiates and initializes a projectile instance in the forward-facing direction.
    /// </summary>
    public abstract void FireProjectile();
}
