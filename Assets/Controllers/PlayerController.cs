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
    public KeyCode shootKey = KeyCode.Space; // RESTORED: Keyboard firing key
    public KeyCode quitKey;

    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 200.0f;

    // RESTORED: Reference link to your separate weapon script
    [Header("Weapon Connection")]
    [Tooltip("Drag the GameObject with your Weapon script here (usually yourself).")]
    [SerializeField] private Weapon playerWeapon;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private float rotationInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Automatically try to find the weapon script on this same object if forgotten
        if (playerWeapon == null)
        {
            playerWeapon = GetComponent<Weapon>();
        }
    }

    void Update()
    {
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

        // 3. RESTORED: CHECK KEYBOARD SHOOTING
        if (Input.GetKeyDown(shootKey))
        {
            ShootProjectile();
        }

        // 4. CHECK QUIT KEY
        if (Input.GetKeyDown(quitKey))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput.normalized * moveSpeed;

        if (rotationInput != 0)
        {
            float targetRotation = rb.rotation + (rotationInput * rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(targetRotation);
        }
    }

    // 5. RESTORED & OPTIMIZED: The bridge function for keyboard and UI Buttons
    public void ShootProjectile()
    {
        if (playerWeapon != null)
        {
            // CHANGE THIS: Replace 'Fire()' with the exact name of your Weapon script's shooting method!
            playerWeapon.Shoot();
        }
        else
        {
            Debug.LogWarning("PlayerController cannot shoot because the Player Weapon script slot is unassigned!");
        }
    }
}
