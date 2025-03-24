using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Zenject;

public class Tower : MonoBehaviour
{
    private TowerData _data;
    private Health _health;

    private float _attackTimer;
    [SerializeField] private Transform _currentTarget;

    [SerializeField] private Transform turretHead;
    [SerializeField] private Transform turretBarrel;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;


    [Header("Anims")] private DOTween[] barrelTweens;
    
    
    [Inject] private TowerManager _towerManager;
    public void Initialize(TowerData data)
    {
        _data = data;
        _attackTimer = 0f;
    }
    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.OnDied += OnDestroyed;
    }

    private void Start()
    {
        _towerManager.RegisterTower(this);
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
            Shoot();
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

    private void Shoot()
    {
        if (projectilePrefab == null) return;

        var projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var projScript = projectile.GetComponent<Projectile>();
        
        if (projScript != null)
        {
            projScript.SetTarget(_currentTarget, _data.damage);
        }

        if (turretHead == null) return;

        PlayTurretRecoil();
        
        Vector3 dir = (_currentTarget.position - turretHead.position).normalized;
        turretHead.forward = dir;
    }
    
    private void PlayTurretRecoil()
    {
        var punchFactor = new Vector3(.25f, .25f, .25f);
        turretHead.DOPunchScale(punchFactor, 0.2f);
        
        float recoilDistance = 0.3f;
        float recoilDuration = 0.035f;
        float returnDistance = 0.75f;
        float returnDuration = 0.65f;

        DOTween.Kill(this);

        turretBarrel.DOLocalMoveZ(recoilDistance, recoilDuration)
            .SetEase(Ease.OutQuad)
            .SetId(this)
            .OnComplete(() =>
            {
                turretBarrel.DOLocalMoveZ(returnDistance, returnDuration)
                    .SetEase(Ease.InQuad)
                    .SetId(this);
            });
    }
    
    public void TakeDamage(int dmg)
    {
        _health.TakeDamage(dmg);
    }
    
    private void OnDestroyed()
    { 
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        if (_towerManager != null)
            _towerManager.UnregisterTower(this);
    }
}
