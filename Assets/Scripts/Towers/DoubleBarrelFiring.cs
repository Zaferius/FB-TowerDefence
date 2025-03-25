using DG.Tweening;
using UnityEngine;

public class DoubleBarrelFiring : MonoBehaviour, ITowerFiringStrategy
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform[] weaponBarrels;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private GameObject projectilePrefab;

    private TowerData _data;
    private float _timer;
    private int _nextFirePointIndex = 0;

    public void Initialize(TowerData data)
    {
        _data = data;
        _timer = 0f;
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;

        var target = EnemyUtils.FindClosestEnemy(transform.position, _data.range);
        if (target != null && _timer <= 0f)
        {
            Fire(target);
            _timer = 1f / _data.fireRate;
        }
    }

    private void Fire(EnemyNavAgent target)
    {
        weaponHolder.transform.DOLookAt(target.transform.position, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            int currentIndex = _nextFirePointIndex % firePoints.Length;

            Transform firePoint = firePoints[currentIndex];
            var projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.SetTarget(target.transform,_data.damage);

            Recoil(currentIndex);

            _nextFirePointIndex++; 
        });
        
    }
    
    private void Recoil(int index)
    {
        if (index < 0 || index >= weaponBarrels.Length) return;

        weaponHolder.transform.DOPunchScale(new Vector3(.125f, .125f, .125f), 0.35f).SetEase(Ease.OutQuad);

        DOTween.Kill(weaponBarrels[index]);

        weaponBarrels[index].DOPunchPosition(
            new Vector3(0, 0, -0.5f), 
            0.4f,                     
            1,                     
            0.5f           
        ).SetEase(Ease.OutQuad).SetId(weaponBarrels[index]);
    }


}