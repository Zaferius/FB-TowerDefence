using UnityEngine;
using Zenject;

public class GridSlot : MonoBehaviour
{
    private ITowerPlacer _towerPlacer;

    [Inject]
    public void Construct(ITowerPlacer placer)
    {
        _towerPlacer = placer;
    }

    private void OnMouseDown()
    {
        _towerPlacer.OpenTowerSelection(this);
    }

}