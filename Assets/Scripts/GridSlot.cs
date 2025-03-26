using DG.Tweening;
using UnityEngine;
using Zenject;

public class GridSlot : MonoBehaviour
{
    private bool _isOccupied = false;
    private bool _isPlaceable = false;
    
    private ITowerPlacer _towerPlacer;
    
    [SerializeField] private Renderer slotRenderer;
    [SerializeField] private Color placeableColor = Color.green;
    [SerializeField] private Color unplaceableColor = Color.gray;

    [Inject]
    public void Construct(ITowerPlacer placer)
    {
        _towerPlacer = placer;
    }

    private void OnMouseDown()
    {
        if (!_isPlaceable || _isOccupied) return;
        _towerPlacer.OpenTowerSelection(this);
    }
    
    private void OnEnable()
    {
        WaveManager.OnPlacementStarted += EnablePlacement;
        WaveManager.OnWaveStarted += DisablePlacementVisual;
    }

    private void OnDisable()
    {
        WaveManager.OnPlacementStarted -= EnablePlacement;
        WaveManager.OnWaveStarted -= DisablePlacementVisual;
    }

    private void EnablePlacement()
    {
        if(_isOccupied) return;
        _isPlaceable = true;
        slotRenderer.material.DOColor(placeableColor, 0.2f);
    }

    private void DisablePlacementVisual(int _)
    {
        _isPlaceable = false;
        slotRenderer.material.DOColor(unplaceableColor, 0.2f);
    }
    
    private void UpdateVisual()
    {
        if (slotRenderer != null)
        {
            slotRenderer.material.color = _isPlaceable && !_isOccupied ? placeableColor : unplaceableColor;
        }
    }
    
    public void SetOccupied()
    {
        _isOccupied = true;
        UpdateVisual();
    }
    
    public void ClearOccupied()
    {
        _isOccupied = false;
        UpdateVisual();
    }

    public bool IsOccupied() => _isOccupied;

}