using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private Transform baseTarget;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private List<EnemyDefinition> availableEnemyTypes;
    [SerializeField] private int totalEnemyCount = 10;

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
        for (int i = 0; i < totalEnemyCount; i++)
        {
            // Random düşman ve spawn noktası seç
            var randomDefinition = availableEnemyTypes[Random.Range(0, availableEnemyTypes.Count)];
            var spawnPoint = spawnPoints[i % spawnPoints.Length];

            _enemyFactory.Create(randomDefinition, baseTarget, spawnPoint.position);
        }
    }
}