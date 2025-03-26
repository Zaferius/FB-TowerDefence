using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private readonly List<EnemyNavAgent> _enemies = new();

    public void RegisterEnemy(EnemyNavAgent enemy)
    {
        if (!_enemies.Contains(enemy))
            _enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyNavAgent enemy)
    {
        _enemies.Remove(enemy);
    }

    public List<EnemyNavAgent> GetAllEnemies()
    {
        return _enemies;
    }
}