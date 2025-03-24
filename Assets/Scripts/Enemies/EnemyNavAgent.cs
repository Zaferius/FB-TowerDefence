using ScriptableObjects;
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
    private Health _health;
    private IEnemyBehavior _behavior;

    [Inject] private TowerManager _towerManager;

    public void Setup(EnemyDefinition definition, Transform target)
    {
        _definition = definition;
        _target = target;
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();

        _health.OnDied += OnDied;
    }

    private void Start()
    {
        _agent.speed = _definition.speed;
        _health.SetMaxHealth(_definition.health);

        // Davranış seçimi
        _behavior = _definition.type switch
        {
            EnemyData.EnemyType.Attacker => new AttackerBehavior(
                _agent,
                _towerManager,
                transform,
                _definition.attackPower,
                _definition.attackRange,
                _definition.attackCooldown),

            EnemyData.EnemyType.Runner => new RunnerBehavior(_agent, _target, transform),

            _ => null
        };

        if (_definition.type is EnemyData.EnemyType.Runner or EnemyData.EnemyType.Attacker)
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
}