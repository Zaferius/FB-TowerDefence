using UnityEngine;
using System;

public class Health: MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float curHealth = 10f;
    public float Max => maxHealth;
    public float Current => curHealth;
    
    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        curHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        curHealth -= amount;
        OnHealthChanged?.Invoke(curHealth, maxHealth);

        if (curHealth <= 0)
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
        curHealth = value;
    }
}