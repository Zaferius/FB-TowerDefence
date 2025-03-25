using UnityEngine;

public static class EnemyUtils
{
    public static EnemyNavAgent FindClosestEnemy(Vector3 position, float range)
    {
        EnemyNavAgent[] enemies = GameObject.FindObjectsOfType<EnemyNavAgent>();
        EnemyNavAgent closest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            var dist = Vector3.Distance(position, enemy.transform.position);
            if (!(dist < range) || !(dist < minDist)) continue;
            minDist = dist;
            closest = enemy;
        }

        return closest;
    }
}