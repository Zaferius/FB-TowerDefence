using System;
using UnityEngine;

public class TowerSelectionPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;

    private bool _isOpen;

    public bool IsOpen => _isOpen;

    private void OnEnable()
    {
        WaveManager.OnWaveStarted += Hide;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveStarted -= Hide;
    }

    public void Show()
    {
        _isOpen = true;
        panelRoot.SetActive(true);
    }

    public void Hide(int _)
    {
        _isOpen = false;
        panelRoot.SetActive(false);
    }
}