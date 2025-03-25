using DG.Tweening;
using UnityEngine;
using Zenject;

public class TowerFactory : IFactory<TowerData, Vector3, Tower>
{
    private readonly DiContainer _container;
    private readonly TowerManager _towerManager;

    public TowerFactory(DiContainer container,TowerManager towerManager)
    {
        _container = container;
        _towerManager = towerManager;
    }

    public Tower Create(TowerData data, Vector3 pos)
    {
        var prefab = data.prefab;
        var obj = _container.InstantiatePrefab(prefab, pos, Quaternion.identity, null);
        var tower = obj.GetComponent<Tower>();

        var strategy = obj.GetComponent<ITowerFiringStrategy>();
        tower.Initialize(data, strategy, _towerManager);
        _towerManager.RegisterTower(tower);

        return tower;
    }
}
