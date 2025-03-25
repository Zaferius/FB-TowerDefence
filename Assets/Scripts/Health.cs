using UnityEngine;
using System;

public class Health: MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 10f;
    public float Current { get; private set; }
    public float Max => maxHealth;

    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        Current = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        Current -= amount;
        OnHealthChanged?.Invoke(Current, maxHealth);

        if (Current <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        Current = value;
    }
}