using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    private bool _hasStarted = false;
    
    public static event Action<int> OnWavesInitialized;
    public static event Action<int> OnWaveStarted;
    public static event Action OnPlacementStarted;
    public static event Action OnGameWin;
    public static  Action OnGameOver;

    [SerializeField] private List<WaveData> waves;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField]  private List<EnemyNavAgent> _aliveEnemies;
    [SerializeField] private Transform baseTarget;

    [SerializeField] private float timeBetweenEnemySpawns = 0.3f;
    [SerializeField] private float placementDuration = 5f;

    private int _currentWaveIndex = 0;
    

    [Inject] private IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent> _enemyFactory;
    [Inject] private EnemyManager _enemyManager;

    public void StartWaveSequence()
    {
        if (_hasStarted) return;
        _hasStarted = true;
        
        OnWavesInitialized?.Invoke(waves.Count);
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (_currentWaveIndex < waves.Count)
        {
            OnPlacementStarted?.Invoke();
            yield return new WaitForSeconds(placementDuration);
            
            OnWaveStarted?.Invoke(_currentWaveIndex + 1);
            yield return StartCoroutine(SpawnWave(waves[_currentWaveIndex]));
            
            yield return new WaitUntil(() => _aliveEnemies.Count <= 0);

            _currentWaveIndex++;
        }
        
        OnGameWin?.Invoke();
        Time.timeScale = 0f;
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        foreach (var entry in wave.entries)
        {
            for (int i = 0; i < entry.count; i++)
            {
                Vector3 spawnPos = GetRandomSpawnPoint();
                var enemy = _enemyFactory.Create(entry.enemyDefinition, baseTarget, spawnPos);

                _aliveEnemies.Add(enemy);
                enemy.Health.OnDeath += () => _aliveEnemies.Remove(enemy);
                
                yield return new WaitForSeconds(timeBetweenEnemySpawns);
            }
        }
    }

    private Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned to WaveManager.");
            return Vector3.zero;
        }

        var index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index].position;
    }
}
