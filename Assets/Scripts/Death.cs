using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Death : MonoBehaviour
{
    public virtual void Die()
    {
        Debug.Log("Entity died. Disabling physics...");
        
        // Disable the collider so it stops triggering OnCollisionEnter2D
        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }

        Destroy(gameObject);
    }

    private void DisableCharacterMovement() { }

}
