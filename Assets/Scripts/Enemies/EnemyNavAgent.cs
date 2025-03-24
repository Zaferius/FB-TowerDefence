using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public class EnemyNavAgent : MonoBehaviour
{
    private EnemyData _data;
    private Transform _target;
    private NavMeshAgent _agent;
    private Health _health;

    public void Setup(EnemyData data, Transform target)
    {
        _data = data;
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
        _agent.speed = _data.speed;
        _agent.SetDestination(_target.position);
        _health.SetMaxHealth(_data.health);
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