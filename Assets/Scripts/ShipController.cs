using UnityEngine;

public class ShipController : MonoBehaviour
{
    // 
    {
    // Always remember variablesbgo at the very top
    [Header(”Possessed Pawn”)]
    public ShipMovement possessedShip;
    Remember unity’s update method runs every frame
    }

    // Update is called once per frame
    void Update()
    {
    // Safety check: if you forgot to drag the ship in, do nothing
    if (possesedShip == null) return;
    }
}
