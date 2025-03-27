using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private HealthBarController healthBarPrefab;
    
    [Header("Scene References")]
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private TowerPlacementManager towerPlacementManager;

    [Header("Enemy Settings")]
    [SerializeField] private EnemyDefinition[] enemyDefinitions;

    public override void InstallBindings()
    {
        // TowerPlacementManager
        Container.Bind<ITowerPlacer>()
            .FromInstance(towerPlacementManager)
            .AsSingle();

        // TowerFactory
        Container.Bind<IFactory<TowerData, Vector3,GridSlot, Tower>>()
            .To<TowerFactory>()
            .AsSingle();

        // EnemyFactory
        Container.Bind<IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent>>()
            .To<EnemyFactory>()
            .AsSingle();

        // EnemyDefinitions
        Container.Bind<EnemyDefinition[]>()
            .FromInstance(enemyDefinitions)
            .AsSingle();

        // TowerManager
        Container.Bind<TowerManager>()
            .FromInstance(towerManager)
            .AsSingle();
        
        Container.Bind<EnemyManager>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<HealthBarController>()
            .FromComponentInNewPrefab(healthBarPrefab)
            .AsTransient();
        
        Container.Bind<WaveManager>().FromComponentInHierarchy().AsSingle();


    }
}