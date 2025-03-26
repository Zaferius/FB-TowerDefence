using System.Collections;
using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private int totalWaves = 5;
    [SerializeField] private float placementDuration = 5f;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private WaveUIController ui;

    public WaveState CurrentState { get; private set; }
    public int CurrentWave { get; private set; }

    public static event Action<int> OnWaveStarted;
    public static event Action OnPlacementStarted;

    
    public void StartGame()
    {
        CurrentWave = 0;
        ui.SetTotalWaves(totalWaves);
        StartCoroutine(BeginNextWave());
    }

    private IEnumerator BeginNextWave()
    {
        CurrentState = WaveState.PlacingTowers;
        OnPlacementStarted?.Invoke();

        Debug.Log("Tower placement phase started.");
        yield return new WaitForSeconds(placementDuration);

        CurrentWave++;
        CurrentState = WaveState.SpawningEnemies;
        OnWaveStarted?.Invoke(CurrentWave);

        Debug.Log($"Wave {CurrentWave} started.");
        enemySpawner.SpawnWave(CurrentWave, OnWaveFinished);
    }

    private void OnWaveFinished()
    {
        StartCoroutine(BeginNextWave());
    }
}
