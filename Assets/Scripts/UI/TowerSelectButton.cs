using UnityEngine;
using Zenject;

public class TowerSelectButton : MonoBehaviour
{
    [SerializeField] private TowerData towerData;

    private ITowerPlacer _towerPlacer;

    [Inject]
    public void Construct(ITowerPlacer placer)
    {
        _towerPlacer = placer;
    }

    public void OnClick()
    {
        _towerPlacer.PlaceSelectedTower(towerData);
    }
}