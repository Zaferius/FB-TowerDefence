using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    public float Current => currentHealth;
    public float Max => maxHealth;

    public event Action OnDied;
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            OnDied?.Invoke();
        }
    }

    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
        currentHealth = amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}