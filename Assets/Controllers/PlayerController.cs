using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Automatically adds Rigidbody2D if missing
public class PlayerController : MonoBehaviour
{
    [Header("Custom Input Settings")]
    public KeyCode moveForward;
    public KeyCode moveBackward;
    public KeyCode strafeLeft;
    public KeyCode strafeRight;
    public KeyCode rotateCounterclockwise;
    public KeyCode rotateClockwise;
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode moveLeft;
    public KeyCode miveRight;
    public KeyCode shootKey = KeyCode.Space;
    public KeyCode quitKey;

    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 200.0f; // Added speed control for rotation

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float rotationInput; // Stores the rotation direction

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Reset inputs every frame
        movementInput = Vector2.zero;
        rotationInput = 0f;

        // 1. READ MOVEMENT KEYCODES
        if (Input.GetKey(moveForward)) { movementInput.y = 1; }
        if (Input.GetKey(moveBackward)) { movementInput.y = -1; }
        if (Input.GetKey(strafeLeft)) { movementInput.x = -1; }
        if (Input.GetKey(strafeRight)) { movementInput.x = 1; }

        // 2. READ ROTATION KEYCODES
        if (Input.GetKey(rotateCounterclockwise)) { rotationInput = 1f; }
        if (Input.GetKey(rotateClockwise)) { rotationInput = -1f; }

        // 3. CHECK KEYBOARD SHOOTING
        if (Input.GetKeyDown(shootKey))
        {
            ShootProjectile();
        }

        // 4. CHECK QUIT KEY
        if (Input.GetKeyDown(quitKey))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Closes play mode in editor
#endif
        }
    }

    void FixedUpdate()
    {
        // 5. MOVE THE SHIP WITH PHYSICS
        rb.linearVelocity = movementInput.normalized * moveSpeed;

        // 6. ROTATE THE SHIP WITH PHYSICS
        if (rotationInput != 0)
        {
            float targetRotation = rb.rotation + (rotationInput * rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(targetRotation);
        }
    }

    // 7. THE FUNCTION FOR YOUR UI BUTTON AND KEYBOARD
    public void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogWarning("Missing Projectile Prefab or Fire Point on PlayerController!");
        }
    }
}