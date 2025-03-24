using UnityEngine;
using Zenject;
using ScriptableObjects;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData runnerData;
    [SerializeField] private EnemyData attackerData;
    [SerializeField] private Transform baseTarget;
    
    [Inject]
    private IFactory<EnemyData, Transform, Vector3, EnemyNavAgent> _factory;

    private void Start()
    {
        _factory.Create(runnerData, baseTarget, new Vector3(0,1,15 + Random.Range(-5f,5f)));
        _factory.Create(attackerData, baseTarget, new Vector3(0,1,15 + Random.Range(-5f,5f)));
    }
}