using System;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    
    [SerializeField] private GameObject youWinPanel;
    [SerializeField] private GameObject youLosePanel;
    [SerializeField] private GameObject wavePanel;
    [SerializeField] private GameObject towerPanel;

    private void OnEnable()
    {
        WaveManager.OnGameWin += ShowWinPanel;
        WaveManager.OnPlacementStarted += SetupGameUI;
        WaveManager.OnGameOver += ShowLosePanel;
    }

    private void OnDisable()
    {
        WaveManager.OnGameWin -= ShowWinPanel;
        WaveManager.OnPlacementStarted -= SetupGameUI;
        WaveManager.OnGameOver -= ShowLosePanel;
    }

    private void Start()
    {
        /*wavePanel.SetActive(false);
        youWinPanel.SetActive(false);*/
        /*youLosePanel.SetActive(false);*/
        /*towerPanel.SetActive(false);*/
    }

    private void SetupGameUI()
    {
        wavePanel.SetActive(true);
        towerPanel.SetActive(true);
    }

    private void ShowWinPanel()
    {
        youWinPanel.SetActive(true);
        wavePanel.SetActive(false);
        towerPanel.SetActive(false);
    }

    private void ShowLosePanel()
    {
        youLosePanel.SetActive(true);
        wavePanel.SetActive(false);
        towerPanel.SetActive(false);
    }
}