using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // This attribute makes the variable show up in your Unity Inspector window
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        // Get the Rigidbody2D component attached to the spaceship
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. READ INPUTS (Forward, Backward, Side-to-Side)
        // GetAxisRaw reads W/S/Up/Down for Vertical and A/D/Left/Right for Horizontal
        movementInput.x = Input.GetAxisRaw("Horizontal"); // Side-to-side (Strafing)
        movementInput.y = Input.GetAxisRaw("Vertical");   // Forward/Backward
    }

    void FixedUpdate()
    {
        // 2. MOVE THE SHIP USING PHYSICS
        // This moves the Rigidbody smoothly based on your inputs and move speed
        rb.linearVelocity = movementInput.normalized * moveSpeed;
    }
}
