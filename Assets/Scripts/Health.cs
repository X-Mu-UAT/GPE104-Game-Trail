using UnityEngine;
using UnityEngine.UI;

/// DESCRIPTION: Handles health tracking and UI updates specifically for the Player Ship.
/// </summary>
public class Health : MonoBehaviour, IHealth
{
    [Header("--- Player Health Settings ---")]
    public int maxHealth = 100;
    public int currentHealth = 100;

    [Header("--- UI Elements ---")]
    [Tooltip("Assign the Screen-Space Player Health Slider UI component here.")]
    public Slider healthSlider;

    private void Start()
    {
        currentHealth = maxHealth;

        // Initialize the screen-space player slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    /// <summary>
    /// Processes standard damage calculations and updates the player HUD.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Clamp health so it never drops below zero
        currentHealth = Mathf.Max(0, currentHealth);
        Debug.Log("Player took damage! Current Health: " + currentHealth);

        // Play the player damage audio clip from the GameManager configurations
        if (GameManager.Instance != null && GameManager.Instance.targetTakeDamageClip != null)
        {
            GameManager.Instance.PlaySoundEffect(GameManager.Instance.targetTakeDamageClip, transform.position);
        }

        // Update the slider visual dynamically when the player takes damage
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Handle Player Death
        if (currentHealth <= 0)
        {
            if (GameManager.Instance != null)
            {
                // Let the GameManager process losing a life and checking game over states
                GameManager.Instance.LoseLife();
            }

            // Find our local death fx tracker
            Death deathComponent = GetComponent<Death>();
            if (deathComponent != null)
            {
                // Pass true for isPlayer, and pass null since there is no Enemy component on the player
                deathComponent.Die(true, null);
            }

            // Instantly restore health variables upon a clean respawn cycle
            ResetHealth();
        }
    }

    /// <summary>
    /// Resets health metrics back to maximum capacity upon player respawning.
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.value = maxHealth;
        }
    }
}