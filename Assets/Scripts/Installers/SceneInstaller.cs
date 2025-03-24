using Zenject;
using UnityEngine;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameSettings gameSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(gameSettings).AsSingle();
    }
}