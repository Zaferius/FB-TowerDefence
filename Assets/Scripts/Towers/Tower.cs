using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour,IDamageable
{
    private IHealth _health;
    private ITowerFiringStrategy _firingStrategy;
    private TowerManager _towerManager;
    private GridSlot _ownerSlot;
    
    private Tween _damageTween;
    
    [SerializeField] private Renderer[] renderersToColorize;
    [SerializeField] private List<EnemyNavAgent> _enemiesInRange = new();
    [SerializeField] private  List<EnemyNavAgent> _currentlyInRange = new();
    [SerializeField] private List<EnemyNavAgent> _attackers = new();
    
    private EnemyNavAgent _currentTarget;
    
    private TowerData _towerData;
    [Inject] private EnemyManager _enemyManager;
    

    [Header("Attack")]
    private float _timer;
    
    
    [Header("Detection System")]
    [SerializeField] private float detectionCheckInterval = 0.2f;
    private float _detectionTimer;
    
    
    public void Initialize(TowerData data, ITowerFiringStrategy strategy, TowerManager towerManager, GridSlot slot)
    {
        _towerData = data;
        _health = GetComponent<IHealth>();
        _firingStrategy = strategy;
        _towerManager = towerManager;
        _towerManager.RegisterTower(this);
        _firingStrategy.Initialize(data);
        _ownerSlot = slot;
        _timer = 0;

        if (_health is Health hc)
        {
            hc.SetMaxHealth(data.health); 
            hc.OnDeath += HandleDeath;
        }
        
        AnimateSpawnColor(_towerData.towerColor);
    }
    
    private void OnEnemyEnterRange(EnemyNavAgent enemy)
    {
        if (!_enemiesInRange.Contains(enemy))
            _enemiesInRange.Add(enemy);
    }

    private void OnEnemyExitRange(EnemyNavAgent enemy)
    {
        _enemiesInRange.Remove(enemy);
        _attackers.Remove(enemy);
    }

    public void RegisterAttacker(EnemyNavAgent enemy)
    {
        var dist = Vector3.Distance(transform.position, enemy.transform.position);
    
        if (dist > _towerData.range) return;

        if (!_attackers.Contains(enemy))
        {
            _attackers.Add(enemy);
        }
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
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            var target = GetPriorityTarget();
            if (target != null)
            {
                _firingStrategy.Fire(target);
                _timer = _towerData.attackTimer;
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
    
    private void AnimateSpawnColor(Color targetColor)
    {
        foreach (var rend in renderersToColorize)
        {
            foreach (var mat in rend.materials)
            {
                mat.color = Color.white;
                mat.DOColor(targetColor, 0.2f).SetEase(Ease.OutQuad);
            }
        }
    }

    public void OnDamaged()
    {
        /*foreach (var rend in renderersToColorize)
        {
            foreach (var mat in rend.materials)
            {
                var originalColor = mat.color;
                
                mat.DOColor(Color.white, 0.1f)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => mat.color = originalColor);
            }
        }*/
        
        transform.DOPunchScale(new Vector3(.1f, .1f, .1f), 0.1f).OnComplete(() =>
        {
            transform.DOScale(new Vector3(1.25f,1.25f,1.25f), 0.15f);
        });
    }
    
    private void HandleDeath()
    {
        _ownerSlot?.ClearOccupied();
        _towerManager.UnregisterTower(this);
        
        Helper.KillAllTweenReferencesIn(transform);
        Destroy(gameObject);
    }
    
}