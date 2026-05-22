using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Always remember variables go at the very top
    [Header("Possessed Pawn")]
    public ShipMovement possessedShip;

    // Update is called once per frame
    void Update()
    {
        // Safety check: if you forgot to drag the ship in, do nothing
        if (possessedShip == null) return;
        // Run our input checks
        HandleLocalControls();
        HandleWorldControls();
    }

    // Add these methods so the calls in Update resolve.
    public void HandleLocalControls()
    {
        // Implement local (ship-relative) input handling here.
        float forwardInput = 0f;
        if (Input.GetKey(KeyCode.W)) forwardInput = 1f;
        if (Input.GetKey(KeyCode.S)) forwardInput = -1f;

        float turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;

        //Check if left Shift or Right Shift is currenlty being held down
        bool turboActive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        possessedShip.MoveLocal(forwardInput, isTurbo: turboActive);
        possessedShip.RotateLocal(turnInput);
    }

    public void HandleWorldControls()
    {
        // Implement world/global input handling here.
        float worldX = 0f;
        float worldY = 0f;
        if (Input.GetKeyDown(KeyCode.UpArrow)) worldY = 1f; //Using GetKeyDown instead of GetKey so that it only moves once per key press, not every frame the key is held down.
        if (Input.GetKeyDown(KeyCode.DownArrow)) worldY = -1f;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) worldX = -1f;
        if (Input.GetKeyDown(KeyCode.RightArrow)) worldX = 1f;

        if (worldX != 0f || worldY != 0f)
        {
            Vector2 worldDirection = new Vector2(worldX, worldY).normalized;
            possessedShip.MoveWorld(worldDirection);
        }
    }
}

