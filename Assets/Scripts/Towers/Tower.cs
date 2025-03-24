using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    private TowerData _data;

    private float _attackTimer;
    private Transform _currentTarget;

    [SerializeField] private Transform turretHead; // Opsiyonel: Dönecek parça
    [SerializeField] private GameObject projectilePrefab; // İleride tanımlayacağız

    public void Initialize(TowerData data)
    {
        _data = data;
        _attackTimer = 0f;
    }

    private void Update()
    {
        if (_data == null) return;

        _attackTimer -= Time.deltaTime;
        
        if (_currentTarget == null || Vector3.Distance(transform.position, _currentTarget.position) > _data.range)
        {
            _currentTarget = FindTarget();
        }
        
        if (_currentTarget != null && _attackTimer <= 0f)
        {
            /*Shoot();*/
            _attackTimer = 1f / _data.fireRate;
        }
    }

    private Transform FindTarget()
    {
        float minDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < _data.range && distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    /*private void Shoot()
    {
        if (projectilePrefab == null) return;

        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.SetTarget(_currentTarget, _data.damage);
        }

        // Opsiyonel: turretHead varsa hedefe döndür
        if (turretHead != null)
        {
            Vector3 dir = (_currentTarget.position - turretHead.position).normalized;
            turretHead.forward = dir;
        }
    }*/
}
