using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    [Header("UI Reference")]
    [Tooltip("Used for the screen-space Player Slider.")]
    public Slider healthSlider;
    [Tooltip("Used for floating World-Space enemy health bars. Assign the Fill Image component here.")]
    public Image healthBarFill;

    [Header("Game Manager Tracking")]
    public bool isPlayer = false;

    [Header("Score Configuration")]
    [Tooltip("How many points the player gets when this object dies.")]
    [SerializeField] private int scoreValue = 100;

    private void Start()
    {
        currentHealth = maxHealth;

        // Initialize the screen-space player slider
        if (isPlayer && healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        // Initialize world-space target fill amount to 100%

        if (!isPlayer && healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f;
        }

        if (!isPlayer && GameManager.Instance != null)
        {
            GameManager.Instance.RegisterObstacle(this);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        // Clamp health so it never drops below zero
        currentHealth = Mathf.Max(0, currentHealth);
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);

        // ADDED: Play the damage sound effect from the GameManager configurations
        if (GameManager.Instance != null && GameManager.Instance.targetTakeDamageClip != null)
        {
            GameManager.Instance.PlaySoundEffect(GameManager.Instance.targetTakeDamageClip, transform.position);

        }

        // Update the slider visual dynamically when the player takes damage
        if (isPlayer && healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Update the target's World-Space health bar fill percentage

        if (!isPlayer && healthBarFill != null)
        {
            // Calculates the fraction between 0.0 and 1.0
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            if (!isPlayer)
            {
                Debug.Log(gameObject.name + " has died and is being destroyed.");
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddPoints(scoreValue);
                    GameManager.Instance.UnregisterObstacle(this);
                }
                Destroy(gameObject);
                return;
            }

            if (isPlayer)
            {
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
        if (isPlayer && healthSlider != null)
        {
            healthSlider.value = maxHealth;
        }

        // Reset target bar visual on health reset