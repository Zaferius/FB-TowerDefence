using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AttackerBehavior : IEnemyBehavior
{
    private readonly NavMeshAgent _agent;
    private readonly TowerManager _towerManager;
    private readonly Transform _self;
    private readonly Transform _baseTarget;

    private readonly int _attackPower;
    private readonly float _attackRange;
    private readonly float _attackCooldown;

    private Tower _targetTower;
    private float _attackTimer;
    
    private readonly IAttackHandler _attackHandler;

    public AttackerBehavior(
        NavMeshAgent agent,
        TowerManager towerManager,
        
        Transform self,
        int attackPower,
        float attackRange,
        float attackCooldown,
        Transform baseTarget,
        IAttackHandler attackHandler)
    {
        _agent = agent;
        _towerManager = towerManager;
        _self = self;
        _baseTarget = baseTarget;

        _attackPower = attackPower;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _attackHandler = attackHandler;
    }

    public void Tick()
    {
        _attackTimer -= Time.deltaTime;

        if (_targetTower == null || _targetTower.gameObject == null)
        {
            _targetTower = FindClosestTower();
            _agent.SetDestination(_targetTower != null ? _targetTower.transform.position : _baseTarget.position);
        }

        if (_targetTower != null)
        {
            float dist = Vector3.Distance(_self.position, _targetTower.transform.position);
            if (dist <= _attackRange && _attackTimer <= 0f)
            {
                _attackHandler.DoAttack(_targetTower);
                _attackTimer = _attackCooldown;
            }
        }
    }

    private Tower FindClosestTower()
    {
        var towers = _towerManager.GetAllTowers();
        Tower closest = null;
        var minDist = Mathf.Infinity;

        foreach (var tower in towers)
        {
            if (tower == null) continue;

            float dist = Vector3.Distance(_self.position, tower.transform.position);
            if (dist < _attackRange * 2f && dist < minDist)
            {
                closest = tower;
                minDist = dist;
            }
        }

        return closest;
    }
}
