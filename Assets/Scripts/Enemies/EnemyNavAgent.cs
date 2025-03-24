using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavAgent : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    [SerializeField] private Transform target;

    private NavMeshAgent _agent;
    [SerializeField] private float _health;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _health = data.health;
        _agent.speed = data.speed;
        _agent.SetDestination(target.position);
    }

    public void TakeDamage(int dmg)
    {
        _health -= dmg;
        if (_health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}