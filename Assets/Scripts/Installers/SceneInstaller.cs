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
        // Tower Placement Manager (UI ve Grid ile kule yerleştirme)
        Container.Bind<ITowerPlacer>()
            .FromInstance(towerPlacementManager)
            .AsSingle(); // veya .IfNotBound(); güvenlik için

        // Tower Factory
        Container.Bind<IFactory<TowerData, Vector3,GridSlot, Tower>>()
            .To<TowerFactory>()
            .AsSingle();

        // Enemy Factory
        Container.Bind<IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent>>()
            .To<EnemyFactory>()
            .AsSingle();

        // Enemy Definitions listesi
        Container.Bind<EnemyDefinition[]>()
            .FromInstance(enemyDefinitions)
            .AsSingle();

        // Tower Manager
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