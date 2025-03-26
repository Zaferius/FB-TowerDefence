using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackerBehavior : IEnemyBehavior
{
    private readonly NavMeshAgent _agent;
    private readonly TowerManager _towerManager;
    private readonly Transform _self;
    private readonly int _attackPower;
    private readonly float _attackRange;
    private readonly float _attackCooldown;
    private readonly float _retargetRange;
    private readonly Transform _baseTarget;
    private readonly IAttackHandler _attackHandler;


    private Tower _currentTargetTower;
    private float _attackTimer;
    
    // 🔁 Yeni hedef aramak için zamanlayıcı
    private float _retargetTimer = 0f;
    private readonly float _retargetCooldown = 1f;

    public AttackerBehavior(NavMeshAgent agent, TowerManager towerManager, Transform self, int attackPower, float attackRange, float attackCooldown, Transform baseTarget, IAttackHandler attackHandler, float retargetRange)
    {
        _agent = agent;
        _towerManager = towerManager;
        _self = self;
        _attackPower = attackPower;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _baseTarget = baseTarget;
        _attackHandler = attackHandler;
        _retargetRange = retargetRange;
    }

    public void Tick()
    {
        _attackTimer -= Time.deltaTime;
        _retargetTimer -= Time.deltaTime;

        // 🔁 Belirli aralıklarla daha yakın kule var mı diye kontrol et
        if (_retargetTimer <= 0f)
        {
            Tower newTarget = FindClosestTower();
            if (newTarget != null && newTarget != _currentTargetTower)
            {
                _currentTargetTower = newTarget;
                _agent.SetDestination(_currentTargetTower.transform.position);
            }

            _retargetTimer = _retargetCooldown;
        }

        if (_currentTargetTower == null)
        {
            _agent.SetDestination(_baseTarget.position);
            return;
        }

        float dist = Vector3.Distance(_self.position, _currentTargetTower.transform.position);

        if (dist <= _attackRange)
        {
            _agent.ResetPath();

            if (_attackTimer <= 0f)
            {
                _attackHandler?.DoAttack(_currentTargetTower);
                _attackTimer = _attackCooldown;
            }
        }
        else
        {
            _agent.SetDestination(_currentTargetTower.transform.position);
        }
    }

    private Tower FindClosestTower()
    {
        List<Tower> towers = _towerManager.GetAllTowers();
        Tower closest = null;
        float minDist = Mathf.Infinity;

        foreach (var tower in towers)
        {
            if (tower == null) continue;

            float dist = Vector3.Distance(_self.position, tower.transform.position);

            if (dist <= _retargetRange && dist < minDist)
            {
                minDist = dist;
                closest = tower;
            }
        }

        return closest;
    }

}
