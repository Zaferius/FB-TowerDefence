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
        IAttackHandler attackHandler)
    {
        _agent = agent;
        _towerManager = towerManager;
        _self = self;
        _attackPower = attackPower;
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _baseTarget = baseTarget;
        _attackHandler = attackHandler;
    }

    public void Tick()
    {
        _attackTimer -= Time.deltaTime;

        // Hedef kule yoksa en yakını ara
        if (_currentTargetTower == null || !_currentTargetTower.gameObject.activeSelf)
        {
            _currentTargetTower = FindClosestTower();

            if (_currentTargetTower != null)
            {
                _currentTargetTower.RegisterAttacker(_self.GetComponent<EnemyNavAgent>());
                _agent.SetDestination(_currentTargetTower.transform.position);
            }
            else
            {
                _agent.SetDestination(_baseTarget.position);
                return;
            }
        }

        float dist = Vector3.Distance(_self.position, _currentTargetTower.transform.position);

        if (dist > _attackRange)
        {
            _agent.SetDestination(_currentTargetTower.transform.position);
            return;
        }

        // Saldırı zamanı
        _agent.ResetPath();

        if (_attackTimer <= 0f)
        {
            _attackHandler?.DoAttack(_currentTargetTower);
            _attackTimer = _attackCooldown;
        }

        // Eğer kule yok edilirse hedefi bırak
        if (_currentTargetTower == null || !_currentTargetTower.gameObject.activeSelf)
        {
            _currentTargetTower?.UnregisterAttacker(_self.GetComponent<EnemyNavAgent>());
            _currentTargetTower = null;
        }
    }

    private Tower FindClosestTower()
    {
        var towers = _towerManager.GetAllTowers();
        Tower closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var tower in towers)
        {
            if (tower == null) continue;

            float dist = Vector3.Distance(_self.position, tower.transform.position);
            if (dist < closestDist)
            {
                closest = tower;
                closestDist = dist;
            }
        }

        return closest;
    }
}
