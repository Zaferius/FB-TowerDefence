using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class EnemyNavAgent : MonoBehaviour
{
    private EnemyDefinition _definition;
    private Transform _target;
    private NavMeshAgent _agent;
    public IHealth Health { get; private set; }
    private Health _health;
    private IEnemyBehavior _behavior;

    [Inject] private TowerManager _towerManager;
    
    [Inject] [SerializeField] private EnemyManager _enemyManager;
    

    public void Setup(EnemyDefinition definition, Transform target)
    {
        _definition = definition;
        _target = target;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        Health = GetComponent<IHealth>();
        
        _health.OnDeath += OnDied;
    }

    private void Start()
    {
        _agent.speed = _definition.speed;
        _health.SetMaxHealth(_definition.health);

        var attackHandler = GetComponent<IAttackHandler>();
        attackHandler?.InitializeFromDefinition(_definition);

        _behavior = _definition.type switch
        {
            EnemyDefinition.EnemyType.Attacker => new AttackerBehavior(
                _agent,
                _towerManager,
                transform,
                _definition.attackPower,
                _definition.attackRange,
                _definition.attackCooldown,
                _target,      
                attackHandler     
            ),

            EnemyDefinition.EnemyType.Runner => new RunnerBehavior(
                _agent,
                _target,
                transform
            ),

            _ => null
        };

        if (_definition.type == EnemyDefinition.EnemyType.Runner)
        {
            _agent.SetDestination(_target.position);
        }
    }

    private void Update()
    {
        _behavior?.Tick();
    }

    public void TakeDamage(int dmg)
    {
        _health.TakeDamage(dmg);
    }

    private void OnDied()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        _enemyManager?.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        _enemyManager?.UnregisterEnemy(this);
    }
}