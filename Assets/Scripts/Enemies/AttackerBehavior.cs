using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackerBehavior : IEnemyBehavior
{
    private readonly NavMeshAgent _agent;
    private readonly TowerManager _towerManager;
    private readonly Transform _selfTransform;
    private readonly float _attackRange;
    private readonly float _attackCooldown;
    private readonly int _attackPower;
    private readonly Transform _baseTarget;
    private readonly IAttackHandler _attackHandler;

    private Tower _currentTargetTower;
    private float _attackTimer;

    public AttackerBehavior(
        NavMeshAgent agent,
        TowerManager towerManager,
        Transform self,
        int attackPower,
        float attackRange,
        float attackCooldown,
        Transform baseTarget,
        IAttackHandler attackHandler
    )
    {
        _agent = agent;
        _towerManager = towerManager;
        _selfTransform = self;
        _attackPower = attackPower;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _baseTarget = baseTarget;
        _attackHandler = attackHandler;
    }

    public void Tick()
    {
        _attackTimer -= Time.deltaTime;

        // Eğer hedef kule null veya yok olmuşsa, en yakın kuleyi bul
        if (_currentTargetTower == null)
        {
            _currentTargetTower = _towerManager.GetClosestTower(_selfTransform.position, _attackRange);

            if (_currentTargetTower != null)
            {
                _agent.SetDestination(_currentTargetTower.transform.position);
            }
            else
            {
                // Etrafta kule yoksa base'e git
                _agent.SetDestination(_baseTarget.position);
                return;
            }
        }

        // Hedef kule hâlâ menzilde mi?
        float dist = Vector3.Distance(_selfTransform.position, _currentTargetTower.transform.position);

        if (dist <= _attackRange)
        {
            _agent.isStopped = true;

            // Kuleye saldıranlar listesine kendini ekle
            _currentTargetTower.RegisterAttacker(GetAgent());

            if (_attackTimer <= 0f)
            {
                _attackHandler?.DoAttack(_currentTargetTower);
                _attackTimer = _attackCooldown;
            }
        }
        else
        {
            // Kuleye yaklaş
            _agent.isStopped = false;
            _agent.SetDestination(_currentTargetTower.transform.position);

            // Eğer çok uzaksa, saldırmayı bırak
            _currentTargetTower.UnregisterAttacker(GetAgent());
        }
    }

    private EnemyNavAgent GetAgent()
    {
        return _selfTransform.GetComponent<EnemyNavAgent>();
    }
}
