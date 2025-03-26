using TMPro;
using UnityEngine;

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
        WaveManager.OnWaveStarted += HandleWaveStarted;
        WaveManager.OnPlacementStarted += HandlePlacementStarted;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveStarted -= HandleWaveStarted;
        WaveManager.OnPlacementStarted -= HandlePlacementStarted;
    }

    public void SetTotalWaves(int total)
    {
        totalWaves = total;
    }

    private void HandleWaveStarted(int waveNumber)
    {
        currentWave = waveNumber;
        isPlacementActive = false;
        waveText.text = $"Wave: {currentWave} / {totalWaves}";
        timerText.text = ""; // temizle
    }

    private void HandlePlacementStarted()
    {
        isPlacementActive = true;
        timer = 5f; // yerleştirme süresi
    }

    private void Update()
    {
        if (!isPlacementActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isPlacementActive = false;
            timerText.text = "";
            return;
        }

        timerText.text = $"Next wave in: {Mathf.CeilToInt(timer)}";
    }
}