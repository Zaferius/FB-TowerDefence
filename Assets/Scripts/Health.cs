using UnityEngine;
using System;
using Zenject;

public class Health: MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float curHealth = 10f;
    public float Max => maxHealth;
    public float Current => curHealth;
    
    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    private void Awake()
    {
        curHealth = maxHealth;
    }
    
    public void TakeDamage(int amount)
    {
        curHealth -= amount;
        OnHealthChanged?.Invoke(curHealth, maxHealth);

        
        if (TryGetComponent<IDamageable>(out var feedback))
        {
            feedback.OnDamaged();
        }
        
        if (curHealth <= 0)
        {
            OnDeath?.Invoke();
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        curHealth = value;
    }

   
}