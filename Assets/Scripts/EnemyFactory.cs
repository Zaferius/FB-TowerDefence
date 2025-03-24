using System.Linq;
using UnityEngine;
using Zenject;
using ScriptableObjects;

public class EnemyFactory : IFactory<EnemyData, Transform, Vector3, EnemyNavAgent>
{
    private readonly DiContainer _container;
    private readonly EnemyTypeConfig[] _typeConfigs;

    public EnemyFactory(EnemyTypeConfig[] configs, DiContainer container)
    {
        _typeConfigs = configs;
        _container = container;
    }

    public EnemyNavAgent Create(EnemyData data, Transform target, Vector3 position)
    {
        var config = _typeConfigs.FirstOrDefault(c => c.type == data.type);
        if (config == null)
        {
            Debug.LogError($"No EnemyTypeConfig found for type: {data.type}");
            return null;
        }

        var obj = _container.InstantiatePrefab(config.prefab, position, Quaternion.identity, null);
        var enemy = obj.GetComponent<EnemyNavAgent>();
        enemy.Setup(data, target);
        return enemy;
    }
}