using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class TowerPlacementManager : MonoBehaviour, ITowerPlacer
{
    [SerializeField] private TowerSelectionPanel selectionPanel;

    private GridSlot _currentSlot;
    private bool _isPlacementMode;

    public bool IsPlacementMode => _isPlacementMode;
    
    private IFactory<TowerData, Vector3, Tower> _towerFactory;
    
    [Inject]
    public void Construct(IFactory<TowerData, Vector3, Tower> towerFactory)
    {
        _towerFactory = towerFactory;
    }
    
    public void EnablePlacement() => _isPlacementMode = true;
    public void DisablePlacement() => _isPlacementMode = false;

    private void Update()
    {
        if (GameStateManager.CurrentState != GameStateManager.GameState.Playing)
            return;
        
        if (selectionPanel.IsOpen && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!hit.collider.TryGetComponent<GridSlot>(out _))
                {
                    selectionPanel.Hide(0);
                    _currentSlot = null;
                }
            }
        }
    }

    public void OpenTowerSelection(GridSlot slot)
    {
        _currentSlot = slot;
        selectionPanel.Show();
    }

    public void PlaceSelectedTower(TowerData data)
    {
        if (_currentSlot == null) return;

        var position = _currentSlot.transform.position;
        _towerFactory.Create(data, position);

        selectionPanel.Hide(0);
        _currentSlot = null;
    }
}
