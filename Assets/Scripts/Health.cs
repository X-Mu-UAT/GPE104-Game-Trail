using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    // ADDED: Checkbox in the inspector to tell the GameManager if this is the player or an obstacle
    [Header("Game Manager Tracking")]
    public bool isPlayer = false;

    private void Start()
    {
        currentHealth = maxHealth;

        // ADDED: If this object is an obstacle, it automatically adds itself to the GameManager's list when the game starts
        if (!isPlayer && GameManager.Instance != null)
        {
            GameManager.Instance.RegisterObstacle(this);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Death deathComponent = GetComponent<Death>();
            if (deathComponent != null)
            {
                // UPDATED: Pass 'isPlayer' and 'this' to the Die method so the death system knows who died
                deathComponent.Die(isPlayer, this);
            }
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + " health reset to full!");
    }
}