using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    [Header("Game Manager Tracking")]
    public bool isPlayer = false;

    private void Start()
    {
        currentHealth = maxHealth;

        // Register asteroids with the GameManager when they spawn
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
            // IF IT IS AN ASTEROID/OBSTACLE
            if (!isPlayer)
            {
                Debug.Log(gameObject.name + " has died and is being destroyed.");

                // FIXED: Tell GameManager this asteroid is gone BEFORE destroying it
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.UnregisterObstacle(this);
                }

                Destroy(gameObject);
                return;
            }

            // IF IT IS THE PLAYER
            if (isPlayer)
            {
                // FIXED: Tell the GameManager to display the Defeat/Game Over UI panel
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.TriggerDefeat();
                }

                Death deathComponent = GetComponent<Death>();
                if (deathComponent != null)
                {
                    deathComponent.Die(isPlayer, this);
                }
                else
                {
                    Debug.LogWarning("Player has no Death component attached!");
                }
            }
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        Debug.Log(gameObject.name + " health reset to full!");
    }
}