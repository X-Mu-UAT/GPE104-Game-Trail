using UnityEngine;

public class Health : MonoBehaviour
{
   public int maxHealth = 100;
    public int currentHealth = 100;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    //Call this function to take damage and reduce health
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            // Get the Death component on this GameObject   
            Death deathComponent = GetComponent<Death>();
            // Check if the Death component exists
            if (deathComponent != null)
            {
                // Tell it to die
                deathComponent.Die();
            }
        }
    }
}
