using DG.Tweening;
using UnityEngine;

public class SingleBarrelFiring : MonoBehaviour, ITowerFiringStrategy
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform weaponBarrel;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    private TowerData _data;
    private float _timer;

    [SerializeField] private EnemyNavAgent _closestEnemy;

    public void Initialize(TowerData data)
    {
        _data = data;
        _timer = 0f;
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;

        var target = EnemyUtils.FindClosestEnemy(transform.position, _data.range);
        _closestEnemy = target;
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
            var projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.SetTarget(target.transform,_data.damage);
            Recoil();
        });
        
    }

    private void Recoil()
    {
        weaponHolder.transform.DOPunchScale(new Vector3(.125f, .125f, .125f), 0.35f).SetEase(Ease.OutQuad);
        
        DOTween.Kill(this);
        weaponBarrel.DOLocalMoveZ(0.3f, 0.05f).SetEase(Ease.OutQuad).SetId(this).OnComplete(() =>
        {
            weaponBarrel.DOLocalMoveZ(0.75f, 0.6f).SetEase(Ease.InQuad).SetId(this).SetDelay(0.35f);
        });
    }
}