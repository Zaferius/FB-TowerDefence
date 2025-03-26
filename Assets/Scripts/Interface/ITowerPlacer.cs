using System;

public interface ITowerPlacer
{
    void OpenTowerSelection(GridSlot slot);
    void PlaceSelectedTower(TowerData data);
    
}