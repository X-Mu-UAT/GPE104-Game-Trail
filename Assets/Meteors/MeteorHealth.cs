using UnityEngine;
using UnityEngine.UI;

public class AsteroidHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = 1f;
            healthSlider.value = 1f;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        // Update slider as percentage
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
