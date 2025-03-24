using System.Linq;
using UnityEngine;
using Zenject;
using ScriptableObjects;

public class EnemyFactory : IFactory<EnemyDefinition, Transform, Vector3, EnemyNavAgent>
{
    private readonly DiContainer _container;

    public EnemyFactory(DiContainer container)
    {
        _container = container;
    }

    public EnemyNavAgent Create(EnemyDefinition definition, Transform target, Vector3 spawnPosition)
    {
        GameObject obj = _container.InstantiatePrefab(definition.prefab, spawnPosition, Quaternion.identity, null);
        var enemy = obj.GetComponent<EnemyNavAgent>();
        enemy.Setup(definition, target);
        return enemy;
    }
}

