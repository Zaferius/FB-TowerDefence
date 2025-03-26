using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private List<Tower> _towers = new();
    
    public event Action<int> OnTowerCountChanged;
    public int GetTowerCount() => _towers.Count;

    public void RegisterTower(Tower tower)
    {
        _towers.Add(tower);
        OnTowerCountChanged?.Invoke(_towers.Count);
    }

    public void UnregisterTower(Tower tower)
    {
        _towers.Remove(tower);
        OnTowerCountChanged?.Invoke(_towers.Count);
    }

    public List<Tower> GetAllTowers()
    {
        return _towers;
    }
}