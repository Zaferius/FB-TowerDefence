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
    
    private float _retargetCooldown = 1f;
    private float _retargetTimer = 0f;

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
        _retargetTimer -= Time.deltaTime;

        
        if (_towerManager.GetAllTowers().Count == 0)
        {
            _agent.SetDestination(_baseTarget.position);
            return;
        }
        
        if ((_currentTargetTower == null || !_currentTargetTower.gameObject.activeSelf) && _retargetTimer <= 0f)
        {
            SetNewTarget();
            _retargetTimer = _retargetCooldown;
        }
        
        if (_currentTargetTower == null)
        {
            _agent.SetDestination(_baseTarget.position);
            return;
        }
        
        float dist = Vector3.Distance(_self.position, _currentTargetTower.transform.position);

        if (dist > _attackRange)
        {
            _agent.SetDestination(_currentTargetTower.transform.position);
            return;
        }
        
        _agent.ResetPath();

        if (_attackTimer <= 0f)
        {
            _attackHandler?.DoAttack(_currentTargetTower);
            _attackTimer = _attackCooldown;
        }
    }


    
    private void SetNewTarget()
    {
        var towers = _towerManager.GetAllTowers();
        float closestDist = Mathf.Infinity;
        Tower closest = null;

        foreach (var tower in towers)
        {
            float dist = Vector3.Distance(_self.position, tower.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = tower;
            }
        }

        if (closest != null)
        {
            _currentTargetTower = closest;
            _currentTargetTower.RegisterAttacker(_self.GetComponent<EnemyNavAgent>());
            _agent.SetDestination(_currentTargetTower.transform.position);
        }
    }
}
