using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameObject towerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<TowerFactory>().AsSingle().WithArguments(towerPrefab);
    }
}