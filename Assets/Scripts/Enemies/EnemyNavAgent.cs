using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class EnemyNavAgent : MonoBehaviour, IDamageable
{
    private EnemyDefinition _definition;
    
    private Transform _target;
    private NavMeshAgent _agent;
   
    private IHealth _health;
    public IHealth Health => _health;
    private IEnemyBehavior _behavior;

    [Inject] private TowerManager _towerManager;
    [Inject] private EnemyManager _enemyManager;
    
    private Vector3 defScale;

    public void Setup(EnemyDefinition definition, Transform target)
    {
        _definition = definition;
        _target = target;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<IHealth>();
        defScale = transform.localScale;
    }

    private void OnEnable()
    {
        _enemyManager?.RegisterEnemy(this);

        if (_health != null)
            _health.OnDeath += OnDied;
    }

    private void OnDisable()
    {
        _enemyManager?.UnregisterEnemy(this);

        if (_health != null)
            _health.OnDeath -= OnDied;
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

    private void OnDied()
    {
        Helper.KillAllTweenReferencesIn(transform);
        Destroy(gameObject);
    }

    public void OnDamaged()
    {
        if(gameObject == null) return;
        transform.DOPunchScale(new Vector3(.1f, .1f, .1f), 0.1f).SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
        {
            transform.DOScale(defScale, 0.15f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        });
    }
    
}
