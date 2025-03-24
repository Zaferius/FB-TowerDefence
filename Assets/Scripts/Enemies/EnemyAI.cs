using UnityEngine;

public interface EnemyAI
{
    void Initialize(Enemy enemy, Transform target);
    void Tick(float deltaTime);
}