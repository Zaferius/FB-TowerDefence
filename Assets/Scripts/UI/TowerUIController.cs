using TMPro;
using UnityEngine;
using Zenject;

public class TowerUIController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI towerCountText;

    [Inject] private TowerManager _towerManager;

    private void OnEnable()
    {
        _towerManager.OnTowerCountChanged += UpdateUI;
        UpdateUI(_towerManager.GetTowerCount());
    }

    private void OnDisable()
    {
        _towerManager.OnTowerCountChanged -= UpdateUI;
    }

    private void UpdateUI(int count)
    {
        towerCountText.text = $"Towers: {count}";
    }
}