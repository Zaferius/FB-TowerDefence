using UnityEngine;

public class Tower : MonoBehaviour
{
    private IHealth _health;
    private ITowerFiringStrategy _firingStrategy;
    private TowerManager _towerManager;

    public void Initialize(TowerData data, ITowerFiringStrategy strategy, TowerManager towerManager)
    {
        _health = GetComponent<IHealth>();
        _firingStrategy = strategy;
        _towerManager = towerManager;
        _firingStrategy.Initialize(data);
        

        if (_health is Health hc)
        {
            hc.SetMaxHealth(data.health); // TowerData'dan geliyorsa
        }
    }

    private void Update()
    {
        _firingStrategy.Tick();
    }
    
    private void OnDestroy()
    {
        _towerManager?.UnregisterTower(this);
    }
}