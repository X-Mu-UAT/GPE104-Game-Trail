using UnityEngine;
using UnityEngine.Events;

public abstract class Death : MonoBehaviour
{
    // UnityEvent to trigger effects when the GameObject dies (particles, sounds, etc.)
    public UnityEvent onDeathEffects;

    // Must be implemented by concrete subclasses (PlayerDeath, EnemyDeath, ...)
    public abstract void Die();

    // Core helper to trigger configured death effects
    protected void TriggerEffects()
    {
        onDeathEffects?.Invoke();
    }
}
