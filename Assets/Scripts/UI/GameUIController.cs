using UnityEngine;

public class GameUIController : MonoBehaviour
{
    
    [SerializeField] private GameObject youWinPanel;
    [SerializeField] private GameObject wavePanel;

    private void OnEnable()
    {
        WaveManager.OnGameWin += ShowWinPanel;
    }

    private void OnDisable()
    {
        WaveManager.OnGameWin -= ShowWinPanel;
    }

    private void ShowWinPanel()
    {
        youWinPanel.SetActive(true);
        wavePanel.SetActive(false);
    }
}