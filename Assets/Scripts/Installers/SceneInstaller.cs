using ScriptableObjects;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private GameObject towerPrefab;
    
    [SerializeField] private EnemyDefinition[] enemyDefinitions;

    public override void InstallBindings()
    {
        Container.Bind<TowerFactory>().AsSingle().WithArguments(towerPrefab);
        
        Container.Bind<TowerManager>().FromInstance(towerManager).AsSingle();
        
        // EnemyFactory
        Container.Bind<IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent>>()
            .To<EnemyFactory>()
            .AsSingle();

        // EnemyDefinitions dizisini dağılacak şekilde kullanmak istersen:
        Container.Bind<EnemyDefinition[]>().FromInstance(enemyDefinitions).AsSingle();


    }
}