using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AttackerBehavior : IEnemyBehavior
{
    private readonly NavMeshAgent _agent;
    private readonly TowerManager _towerManager;
    private readonly Transform _self;
    [SerializeField] private readonly int _attackPower;
    private readonly float _attackRange;
    private readonly float _attackCooldown;

    [SerializeField] private Tower _targetTower;
    [SerializeField] private float _attackTimer;

    public AttackerBehavior(NavMeshAgent agent, TowerManager towerManager, Transform self,int attackPower,float attackRange, float attackCooldown)
    {
        _agent = agent;
        _towerManager = towerManager;
        _self = self;
        _attackPower = attackPower;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
    }

    public void Tick()
    {
        _attackTimer -= Time.deltaTime;

        if (_targetTower == null)
        {
            _targetTower = FindClosestTower();
            if (_targetTower != null)
                _agent.SetDestination(_targetTower.transform.position);
        }
        else
        {
            float dist = Vector3.Distance(_self.position, _targetTower.transform.position);

            if (dist <= _attackRange)
            {
                if (_attackTimer <= 0f)
                {
                    _targetTower.TakeDamage(_attackPower);
                    _attackTimer = _attackCooldown;
                }
            }
            else
            {
                _agent.SetDestination(_targetTower.transform.position);
            }
        }
    }

    private Tower FindClosestTower()
    {
        List<Tower> towers = _towerManager.GetAllTowers();
        Tower closest = null;
        float minDist = Mathf.Infinity;

        foreach (var tower in towers)
        {
            float dist = Vector3.Distance(_self.position, tower.transform.position);
            if (dist < _attackRange && dist < minDist)
            {
                closest = tower;
                minDist = dist;
            }
        }

        return closest;
    }
}