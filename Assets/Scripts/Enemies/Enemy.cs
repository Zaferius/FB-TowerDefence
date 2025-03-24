using System;
using ScriptableObjects;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _data;
    private float _currentHealth;
    private Transform _target;
    private EnemyAI _ai;
    public bool testEnemy;

    private void Awake()
    {
        if (testEnemy)
        {
            _currentHealth = 100;
        }
    }

    public void Initialize(EnemyData data, Transform target)
    {
        _data = data;
        _target = target;

        if (_data.type == EnemyData.EnemyType.Runner)
            _ai = new RunnerAI();
        else if (_data.type == EnemyData.EnemyType.Attacker)
            _ai = new AttackerAI();

        _ai.Initialize(this, _target);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        _ai?.Tick(Time.deltaTime);
    }

    public EnemyData GetData() => _data;
}