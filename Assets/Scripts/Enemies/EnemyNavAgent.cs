using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class EnemyNavAgent : MonoBehaviour
{
    private Transform _target;
    private NavMeshAgent _agent;
    private Health _health;
    
    private EnemyDefinition _definition;

    public void Setup(EnemyDefinition definition, Transform target)
    {
        _definition = definition;
        _target = target;
    }
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _health.OnDied += OnDeath;
    }

    private void Start()
    {
        _agent.SetDestination(_target.position);
        _agent.speed = _definition.speed;
        _health.SetMaxHealth(_definition.health);
    }

    private void Update()
    {
        // ..AI
    }

    public void TakeDamage(int dmg)
    {
        _health.TakeDamage(dmg);
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}