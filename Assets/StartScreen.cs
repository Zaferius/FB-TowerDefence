using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using System;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject touchToText;
    private WaveManager _waveManager;
    
    public event Action OnStartGame;

    [Inject]
    public void Construct(WaveManager waveManager)
    {
        _waveManager = waveManager;
    }

    public void OnStartButtonPressed()
    {
        GameStateManager.StartGame();

        touchToText.SetActive(false);
        canvasGroup.DOFade(0, 1).OnComplete(() =>
        {
            canvasGroup.gameObject.SetActive(false);
        });
        
        
        
        _waveManager.StartWaveSequence();
    }
}

