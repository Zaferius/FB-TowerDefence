using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    private readonly List<Tower> _towers = new();

    public void RegisterTower(Tower tower)
    {
        if (!_towers.Contains(tower))
            _towers.Add(tower);
    }

    public void UnregisterTower(Tower tower)
    {
        if (_towers.Contains(tower))
            _towers.Remove(tower);
    }

    public List<Tower> GetAllTowers()
    {
        return _towers;
    }
    
    public Tower GetClosestTower(Vector3 position, float maxDistance = Mathf.Infinity)
    {
        Tower closest = null;
        float closestDist = maxDistance;

        foreach (var tower in _towers)
        {
            float dist = Vector3.Distance(position, tower.transform.position);
            if (dist < closestDist)
            {
                closest = tower;
                closestDist = dist;
            }
        }

        return closest;
    }
}