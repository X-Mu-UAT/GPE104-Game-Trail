using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int damageAmount = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DealDamage(other.gameObject);
    }
    {
        DealDamage(collision.gameObject);
    }
    private void DealDamage(GameObject objectHit)
    {
        Health targetHealth = objectHit.GetComponent<Health>();

        if (targetHealth !=null

        {
            targetHealth.TakeDamage(damageAmount);
            Debug.Log(gameObject.name + “dealt damage to “ + objectHir.name);
        }
    }
}
