using TMPro;
using UnityEngine;
using System;

public class WaveUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI timerText;

    private int totalWaves;
    private int currentWave;
    private bool isPlacementActive;
    private float timer;

    private void OnEnable()
    {
        WaveManager.OnWavesInitialized += SetTotalWaves;
        WaveManager.OnWaveStarted += HandleWaveStarted;
        WaveManager.OnPlacementStarted += HandlePlacementStarted;
    }

    private void OnDisable()
    {
        WaveManager.OnWavesInitialized -= SetTotalWaves;
        WaveManager.OnWaveStarted -= HandleWaveStarted;
        WaveManager.OnPlacementStarted -= HandlePlacementStarted;
    }

    private void SetTotalWaves(int total)
    {
        totalWaves = total;
        currentWave = 0;
        UpdateWaveText();
    }

    private void HandleWaveStarted(int waveNumber)
    {
        currentWave = waveNumber;
        isPlacementActive = false;
        UpdateWaveText();
        timerText.text = "";
    }

    private void HandlePlacementStarted()
    {
        isPlacementActive = true;
        timer = 5f;
    }

    private void Update()
    {
        if (!isPlacementActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isPlacementActive = false;
            timerText.text = "";
        }
        else
        {
            timerText.text = $"Next wave in: {Mathf.CeilToInt(timer)}";
        }
    }

    private void UpdateWaveText()
    {
        waveText.text = $"Wave: {currentWave} / {totalWaves}";
    }
}