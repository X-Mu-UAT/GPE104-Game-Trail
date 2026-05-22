using UnityEngine;

public class ShipMovement : MonoBehaviour
{
//These are the variables for the movement. they always go at the very top
[Header(”Movement Speeds”)]
[Tooltip(”Units per second for local movement.”)]
public float localspeed = 5f;
[Tooltip(”Degrees per second for local rotation.”)]
public float rotationSpeed = 180f;
[Tooltip(”Units per second for world movement.”)]
public float worldSpeed = 5f

//Custom methods go below the variables
// There is no void start() or void Update() for this

//Action 1: Handles WASD moving and sliding
public void MoveLocal(float verticalInput)
    {
        transform.Translate(Vector3.up * verticalInput * localSpeed* Time.deltaTime, Space.Self);
    }
//Action 2: Handles Turing left/right
public void RotateLocal(float horizontalInput)
{
transform.Rotate(Vector3.forward * -horizontalInput * rotationSpeed * Time.deltaTime);
}
//Action 3: Handles Arrow keys moving globally
public void MoveWorld(Vector2 direction)
    {
        
    }
}
