using System.Linq;
using UnityEngine;
using Zenject;
public class EnemyFactory : IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent>
{
    private readonly DiContainer _container;

    public EnemyFactory(DiContainer container)
    {
        _container = container;
    }

    public EnemyNavAgent Create(EnemyDefinition definition, Transform target, Vector3 spawnPosition)
    {
        var obj = _container.InstantiatePrefab(definition.prefab, spawnPosition, Quaternion.identity, null);
        var enemy = obj.GetComponent<EnemyNavAgent>();
        enemy.Setup(definition, target);
        return enemy;
    }
}

