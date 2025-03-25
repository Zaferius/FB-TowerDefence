using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
    private IHealth _health;
    private ITowerFiringStrategy _firingStrategy;
    private TowerManager _towerManager;
    [Inject] private EnemyManager _enemyManager;
    
    [SerializeField] private List<EnemyNavAgent> _enemiesInRange = new();
    [SerializeField] private  List<EnemyNavAgent> _currentlyInRange = new();
    [SerializeField] private List<EnemyNavAgent> _attackers = new();
    
    private EnemyNavAgent _currentTarget;
    
    private TowerData _towerData;

    [SerializeField] private float attackCooldown = 1f;
    private float _attackTimer;
    
    [SerializeField] private float detectionCheckInterval = 0.2f; // kaç saniyede bir tarasın
    private float _detectionTimer;
    
    
    public void Initialize(TowerData data, ITowerFiringStrategy strategy, TowerManager towerManager)
    {
        _towerData = data;
        _health = GetComponent<IHealth>();
        _firingStrategy = strategy;
        _towerManager = towerManager;
        _towerManager.RegisterTower(this);
        _firingStrategy.Initialize(data);
        
        

        if (_health is Health hc)
        {
            hc.SetMaxHealth(data.health); // TowerData'dan geliyorsa
        }
    }
    
    public void OnEnemyEnterRange(EnemyNavAgent enemy)
    {
        if (!_enemiesInRange.Contains(enemy))
            _enemiesInRange.Add(enemy);
    }

    public void OnEnemyExitRange(EnemyNavAgent enemy)
    {
        _enemiesInRange.Remove(enemy);
        _attackers.Remove(enemy);
    }

    public void RegisterAttacker(EnemyNavAgent enemy)
    {
        if (!_attackers.Contains(enemy))
            _attackers.Add(enemy);
    }

    public void UnregisterAttacker(EnemyNavAgent enemy)
    {
        _attackers.Remove(enemy);
    }
    
    private void UpdateEnemyRangeDetection()
    {
        var allEnemies = _enemyManager.GetAllEnemies();
        var newInRange = new List<EnemyNavAgent>();

        foreach (var enemy in allEnemies)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= _towerData.range)
            {
                newInRange.Add(enemy);

                if (!_currentlyInRange.Contains(enemy))
                {
                    _currentlyInRange.Add(enemy);
                    OnEnemyEnterRange(enemy);
                }
            }
        }

        for (int i = _currentlyInRange.Count - 1; i >= 0; i--)
        {
            var enemy = _currentlyInRange[i];
            if (!newInRange.Contains(enemy))
            {
                _currentlyInRange.Remove(enemy);
                OnEnemyExitRange(enemy);
            }
        }
    }
    private void Update()
    {
        _detectionTimer -= Time.deltaTime;

        if (_detectionTimer <= 0f)
        {
            UpdateEnemyRangeDetection();
            _detectionTimer = detectionCheckInterval;
        }
        
        UpdateAttack(); 
    }

    private void UpdateAttack()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0f)
        {
            var target = GetPriorityTarget();
            if (target != null)
            {
                _firingStrategy.Fire(target);
                _attackTimer = attackCooldown;
            }
        }
    }

    private EnemyNavAgent GetPriorityTarget()
    {
        if (_attackers.Count > 0)
        {
            return _attackers
                .Where(e => e != null).OrderBy(e => e.Health.Current)
                .FirstOrDefault();
        }

        return _enemiesInRange
            .Where(e => e != null)
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .FirstOrDefault();
    }
    
    private void OnDestroy()
    {
        _towerManager?.UnregisterTower(this);
    }
}