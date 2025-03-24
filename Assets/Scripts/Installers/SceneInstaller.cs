using ScriptableObjects;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject towerPrefab;
    
    [SerializeField] private EnemyTypeConfig[] enemyTypeConfigs;

    public override void InstallBindings()
    {
        Container.Bind<TowerFactory>().AsSingle().WithArguments(towerPrefab);
        
        Container.Bind<IFactory<EnemyData, Transform, Vector3, EnemyNavAgent>>()
            .To<EnemyFactory>()
            .AsSingle()
            .WithArguments((object)enemyTypeConfigs);

    }
}