using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Call this function to take damage and reduce health 
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);

        // FIXED: The spaceship will now ONLY die if health is 0 or less
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

    // ADDED: Call this from your Death script to heal back to full
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + " health reset to full!");
    }
}
