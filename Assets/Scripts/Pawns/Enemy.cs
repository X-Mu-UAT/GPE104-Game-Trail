using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Base Configurations")]
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int pointsValue = 100;
    [SerializeField] protected float speed = 2f;

    [Header("UI Component Bindings")]

    private Slider healthSlider;
    private GameObject spawnedHealthBar;
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private GameObject worldSpaceCanvas;


    protected int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        // Spawn health bar automatically
        if (healthBarPrefab != null)
        {
            spawnedHealthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);

            // Make it follow this enemy

            // Get slider reference from prefab
            healthSlider = spawnedHealthBar.GetComponentInChildren<Slider>();
        }

        UpdateHealthBarUI();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterObstacle(this);
        }

    }

    protected virtual void Update()
    {
        // Enforce boundary space warping across all enemy elements
        if (GameManager.Instance != null)
        {
            transform.position = GameManager.Instance.WrapPosition(transform.position);
        }

        MaintainCanvasFacing();
    }

    public virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        // Clamp system to verify health values never fall below zero
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBarUI();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlaySoundEffect(GameManager.Instance.targetTakeDamageClip, transform.position);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBarUI()
    {
        if (healthSlider != null)
        {
            // Native normalized scaling matching your 0% - 100% display rules
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private void MaintainCanvasFacing()
    {
        // Prevents the floating health elements from rolling upside down if the target entity spins
        if (worldSpaceCanvas != null)
        {
            worldSpaceCanvas.transform.rotation = Quaternion.identity;
        }
    }

    protected virtual void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddPoints(pointsValue);
            GameManager.Instance.PlaySoundEffect(GameManager.Instance.targetDeathClip, transform.position);
            GameManager.Instance.UnregisterObstacle(this);
        }

        Destroy(gameObject);
    }
}

