using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public bool isInstantKill = false;
// How much damage this object deals if it is not an instant kill
    public int damageAmount = 25;

    private void OnTriggerEnter2D(Collider2D other)
    {
        DealDamage(other.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DealDamage(collision.gameObject);
    }
    private void DealDamage(GameObject objectHit)
    {
        Health targetHealth = objectHit.GetComponent<Health>();

        if (targetHealth == null)
        {
            return;
        }
        if (isInstantKill)
        {
            Death deathComponent = objectHit.GetComponent<Death>();
            if (deathComponent != null)
            {
                Debug.Log(gameObject.name + " instantly killed " + objectHit.name);
                deathComponent.Die();
            }
            return;
        }
    targetHealth.TakeDamage(damageAmount);
}
}

