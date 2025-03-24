using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform baseTarget;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private EnemyDefinition[] enemyDefinitions;

    private IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent> _enemyFactory;

    [Inject]
    public void Construct(IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent> factory)
    {
        _enemyFactory = factory;
    }

    private void Start()
    {
        SpawnAllEnemies();
    }

    private void SpawnAllEnemies()
    {
        for (int i = 0; i < enemyDefinitions.Length; i++)
        {
            var definition = enemyDefinitions[i];
            var spawnPoint = spawnPoints[i % spawnPoints.Length]; // circular

            _enemyFactory.Create(definition, baseTarget, spawnPoint.position);
        }
    }
}