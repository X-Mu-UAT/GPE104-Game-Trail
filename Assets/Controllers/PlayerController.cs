using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Custom Input Settings")]
    public KeyCode moveForward;    // Will show up as a dropdown in Inspector
    public KeyCode moveBackward;   // Will show up as a dropdown in Inspector
    public KeyCode strafeLeft;     // Will show up as a dropdown in Inspector
    public KeyCode strafeRight;    // Will show up as a dropdown in Inspector
    public KeyCode rotateCounterclockwise;
    public KeyCode rotateClockwise;

    // World space movement keys
    public KeyCode moveUp
    public KeyCode moveDown
    public KeyCode moveLeft
    public KeyCode miveRight

    //Quit key
    public KeyCode quitKey;


    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Reset inputs every frame
        movementInput = Vector2.zero;

        // 1. READ INDIVIDUAL KEYCODES
        if (Input.GetKey(moveForward))
        {
            movementInput.y = 1; // Move Up / Forward
        }
        if (Input.GetKey(moveBackward))
        {
            movementInput.y = -1; // Move Down / Backward
        }
        if (Input.GetKey(strafeLeft))
        {
            movementInput.x = -1; // Move Left / Strafe Left
        }
        if (Input.GetKey(strafeRight))
        {
            movementInput.x = 1; // Move Right / Strafe Right
        }
    }

    void FixedUpdate()
    {
        // 2. MOVE THE SHIP WITH PHYSICS
        rb.linearVelocity = movementInput.normalized * moveSpeed;
    }
}