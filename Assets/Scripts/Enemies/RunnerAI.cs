using UnityEngine;

public class RunnerAI : EnemyAI
{
    private Enemy _enemy;
    private Transform _target;
    private float _speed;

    public void Initialize(Enemy enemy, Transform target)
    {
        _enemy = enemy;
        _target = target;
        _speed = enemy.GetData().speed;
    }

    public void Tick(float deltaTime)
    {
        if (_target == null) return;

        Vector3 dir = (_target.position - _enemy.transform.position).normalized;
        _enemy.transform.position += dir * _speed * deltaTime;
    }
}