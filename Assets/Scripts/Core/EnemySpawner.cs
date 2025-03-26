using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private Transform baseTarget;
    [SerializeField] private EnemyDefinition[] waveEnemies;
    [SerializeField] private Transform[] spawnPoints;
    [Inject] private IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent> _enemyFactory;

    [SerializeField] private int _aliveEnemies;
    private Action _onWaveComplete;
    
    public void SpawnWave(int waveNumber, Action onWaveComplete)
    {
        _onWaveComplete = onWaveComplete;
        _aliveEnemies = waveEnemies.Length;

        for (int i = 0; i < waveEnemies.Length; i++)
        {
            var def = waveEnemies[i];
            var spawnPoint = spawnPoints[i % spawnPoints.Length];
            var enemy = _enemyFactory.Create(def, baseTarget, spawnPoint.position);
            
            var health = enemy.GetComponent<IHealth>();
            if (health != null)
            {
                health.OnDeath += HandleEnemyDeath;
            }
            
        }
    }

    private void HandleEnemyDeath()
    {
        print("Enemy Died");
        _aliveEnemies--;
        if (_aliveEnemies <= 0)
        {
            _onWaveComplete?.Invoke();
        }
    }
}