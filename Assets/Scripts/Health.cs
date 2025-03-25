using UnityEngine;
using System;
using Zenject;

public class Health: MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float curHealth = 10f;
    public float Max => maxHealth;
    public float Current => curHealth;

    /*[Inject] private DiContainer _container;
    [Inject] private HealthBarController _prefab;*/
    
    public event Action OnDeath;
    public event Action<float, float> OnHealthChanged;

    private void Awake()
    {
        curHealth = maxHealth;
    }

    private void Start()
    {
        /*TryAttachHealthBar();*/
    }
    
    /*private void TryAttachHealthBar()
    {
        if (GetComponent<HealthBarHandler>() == null)
        {
           var hpHandler = gameObject.AddComponent<HealthBarHandler>();
           hpHandler.healthBarPrefab = _prefab;
        }
    }*/

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