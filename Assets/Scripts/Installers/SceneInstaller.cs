using ScriptableObjects;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject towerPrefab;
    
    [SerializeField] private EnemyDefinition[] enemyDefinitions;

    public override void InstallBindings()
    {
        Container.Bind<TowerFactory>().AsSingle().WithArguments(towerPrefab);
        
        // EnemyFactory
        Container.Bind<IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent>>()
            .To<EnemyFactory>()
            .AsSingle();

        // EnemyDefinitions dizisini dağılacak şekilde kullanmak istersen:
        Container.Bind<EnemyDefinition[]>().FromInstance(enemyDefinitions).AsSingle();


    }
}