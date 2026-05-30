using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    public bool isInstantKill = false;
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

        // Safety Check: If the object hit doesn't have a Health script, do nothing
        if (targetHealth == null)
        {
            return;
        }

        if (isInstantKill)
        {
            Debug.Log(gameObject.name + " instantly killed " + objectHit.name);

            // CRASH FIX: Instead of calling .Die() directly, pass massive damage 
            // to the Health script so it cleanly triggers your Defeat screen logic.
            targetHealth.TakeDamage(999999);
            return;
        }

        targetHealth.TakeDamage(damageAmount);
    }
}
