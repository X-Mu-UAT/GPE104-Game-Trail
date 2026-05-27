using UnityEngine;

public class TestDamage : MonoBehaviour
{
    private Health healthComponent;
    private void Start()

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    {
        healthComponent = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (healthComponent == null)
            {
            }
            healthComponent.TakeDamage(25);
            Debug.Log("Took 25 damage, current health: " + healthComponent.currentHealth);
        }
    }
}
