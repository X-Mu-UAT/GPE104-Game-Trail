using UnityEngine;

public class ShipMovement : MonoBehaviour
{
//These are the variables for the movement. they always go at the very top
[Header("Movement Speeds")]
[Tooltip("Units per second for local movement.")]
public float localSpeed = 5f;
    [Tooltip("Degrees per second for local rotation.")]
    public float turboSpeed = 10f;
    // New Variable for turbo speed
    public float rotationSpeed = 180f;
[Tooltip("Units per second for world movement.")]
public float worldSpeed = 5f;

//Custom methods go below the variables
// There is no void start() or void Update() for this

//Action 1: Handles WASD moving and sliding
public void MoveLocal(float verticalInput, bool isTurbo)
    {
        float currentSpeed = isTurbo ? turboSpeed : localSpeed;
        transform.Translate(Vector3.up * verticalInput * currentSpeed * Time.deltaTime, Space.Self);
    }
//Action 2: Handles Turing left/right""
public void RotateLocal(float horizontalInput)
{
transform.Rotate(Vector3.forward * -horizontalInput * rotationSpeed * Time.deltaTime);
}
//Action 3: Handles Arrow keys moving globally
public void MoveWorld(Vector2 direction)
    {
        Vector3 worldDirection = new Vector3(direction.x, direction.y, 0f);
        transform.Translate(worldDirection * worldSpeed * Time.deltaTime, Space.World);
    }
}// This curly brace closes the whole script
