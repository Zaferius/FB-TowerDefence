using UnityEngine;

public class AttackerAI : EnemyAI
{
    private Enemy _enemy;
    private Transform _target;
    private float _speed;
    private float attackRange = 2f;
    private float attackCooldown = 1f;
    private float attackTimer;

    public void Initialize(Enemy enemy, Transform target)
    {
        _enemy = enemy;
        _target = target;
        _speed = enemy.GetData().speed;
        attackTimer = 0f;
    }

    public void Tick(float deltaTime)
    {
        if (_target == null) return;

        float distance = Vector3.Distance(_enemy.transform.position, _target.position);

        if (distance > attackRange)
        {
            Vector3 dir = (_target.position - _enemy.transform.position).normalized;
            _enemy.transform.position += dir * _speed * deltaTime;
        }
        else
        {
            attackTimer -= deltaTime;
            if (attackTimer <= 0f)
            {
                attackTimer = attackCooldown;
            }
        }
    }
}