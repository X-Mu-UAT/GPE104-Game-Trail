using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Possessed Pawn")]
    public ShipMovement possessedShip;
    private ShooterBullet shipShooter; // Reference to the shooter component

    void Start()
    {
        UpdatePawnReferences();
    }

    void Update()
    {
        if (possessedShip == null) return;

        HandleLocalControls();
        HandleWorldControls();
        HandleFiring();
    }

    // Automatically grab the shooter component when a ship is linked
    public void UpdatePawnReferences()
    {
        if (possessedShip != null)
        {
            shipShooter = possessedShip.GetComponent<ShooterBullet>();
        }
    }

    public void HandleLocalControls()
    {
        float forwardInput = 0f;
        if (Input.GetKey(KeyCode.W)) forwardInput = 1f;
        if (Input.GetKey(KeyCode.S)) forwardInput = -1f;

        float turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;

        bool turboActive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        possessedShip.MoveLocal(forwardInput, isTurbo: turboActive);
        possessedShip.RotateLocal(turnInput);
    }

    public void HandleWorldControls()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) possessedShip.MoveWorld(new Vector2(0, 1));
        if (Input.GetKeyDown(KeyCode.DownArrow)) possessedShip.MoveWorld(new Vector2(0, -1));
        if (Input.GetKeyDown(KeyCode.LeftArrow)) possessedShip.MoveWorld(new Vector2(-1, 0));
        if (Input.GetKeyDown(KeyCode.RightArrow)) possessedShip.MoveWorld(new Vector2(1, 0));
    }

    // NEW: Handle Firing Input
    private void HandleFiring()
    {
        // Spacebar acts as the Fire Button
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shipShooter != null)
            {
                shipShooter.Shoot();
            }
        }
    }
}