using UnityEngine;

/// <summary>
/// Monitors keyboard input patterns and issues commands to an assigned spaceship Pawn.
/// Uses vector calculations instead of physics components per course guidelines.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Pawn Connection Assignment")]
    [Tooltip("Drag the GameObject containing your SpaceShipPawn component into this field.")]
    [SerializeField] private Pawn targetPawn;

    [Header("Continuous Input Key Mappings (WASD)")]
    public KeyCode moveForward = KeyCode.W;
    public KeyCode moveBackward = KeyCode.S;
    public KeyCode rotateCounterClockwise = KeyCode.A;
    public KeyCode rotateClockwise = KeyCode.D;

    [Header("Discrete Teleport Jumps (Arrow Keys & T)")]
    public KeyCode teleportUp = KeyCode.UpArrow;
    public KeyCode teleportDown = KeyCode.DownArrow;
    public KeyCode teleportLeft = KeyCode.LeftArrow;
    public KeyCode teleportRight = KeyCode.RightArrow;
    public KeyCode randomTeleportKey = KeyCode.T;

    [Header("Designer Specified Teleport Matrix")]
    [Tooltip("Distance traveled instantly in world space when clicking an arrow key.")]
    [SerializeField] private float teleportDistance = 3.0f;

    [Header("Action Key Mappings")]
    public KeyCode shootKey = KeyCode.Space;
    public KeyCode quitKey = KeyCode.Escape;

    private void Start()
    {
        // Automatically check local hierarchy elements if left blank in inspector
        if (targetPawn == null)
        {
            targetPawn = GetComponent<Pawn>();
        }

        if (targetPawn == null)
        {
            Debug.LogError("[PlayerController] Critical assignment error: No Target Pawn linked!");
        }
    }

    private void Update()
    {
        if (targetPawn == null) return;

        HandleContinuousMovementInput();
        HandleDiscreteTeleportInput();
        HandleActionInputKeys();
    }

    /// <summary>
    /// Evaluates continuous directional inputs and tracks Shift scaling conditions.
    /// </summary>
    private void HandleContinuousMovementInput()
    {
        float forwardInput = 0f;
        float rotationInput = 0f;

        // Process translation vector axis conditions
        if (Input.GetKey(moveForward)) forwardInput = 1f;
        if (Input.GetKey(moveBackward)) forwardInput = -1f;

        // Process rotational angular shift criteria
        if (Input.GetKey(rotateCounterClockwise)) rotationInput = -1f; // Standard negative Z rotation math loop
        if (Input.GetKey(rotateClockwise)) rotationInput = 1f;

        // Requirement: Detect Turbo contexts using either left or right shift indicators
        bool isTurboActive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Forward finalized commands over to the Pawn execution context
        targetPawn.MoveLocal(forwardInput, rotationInput, isTurboActive);
    }

    /// <summary>
    /// Tracks single-click discrete warp steps and processes boundaries safely.
    /// </summary>
    private void HandleDiscreteTeleportInput()
    {
        // Requirement: Execute once per single isolated keypress cycle (GetKeyDown)
        if (Input.GetKeyDown(teleportUp))
        {
            targetPawn.TeleportWorld(Vector3.up * teleportDistance);
        }
        if (Input.GetKeyDown(teleportDown))
        {
            targetPawn.TeleportWorld(Vector3.down * teleportDistance);
        }
        if (Input.GetKeyDown(teleportLeft))
        {
            targetPawn.TeleportWorld(Vector3.left * teleportDistance);
        }
        if (Input.GetKeyDown(teleportRight))
        {
            targetPawn.TeleportWorld(Vector3.right * teleportDistance);
        }

        // Requirement: Random layout shuffle bounds trigger via the T Key
        if (Input.GetKeyDown(randomTeleportKey))
        {
            targetPawn.TeleportRandom();
        }
    }

    /// <summary>
    /// Processes firing triggers and system actions.
    /// </summary>
    private void HandleActionInputKeys()
    {
        if (Input.GetKeyDown(shootKey))
        {
            targetPawn.FireProjectile();
        }

        if (Input.GetKeyDown(quitKey))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.QuitToDesktop();
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
