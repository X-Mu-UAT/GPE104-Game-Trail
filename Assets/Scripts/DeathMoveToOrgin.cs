using UnityEngine;

public class DeathMoveToOrgin : Death
{
    public Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    public override void Die()
    {
        
        transform.position = startPosition;
    }
}
