using DG.Tweening;
using UnityEngine;

public class DoubleBarrelFiring : MonoBehaviour, ITowerFiringStrategy
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform[] weaponBarrels;
    [SerializeField] private Transform[] firePoints;

    private TowerData _data;
    private float _timer;
    private int _nextFirePointIndex = 0;

    public void Initialize(TowerData data)
    {
        _data = data;
    }
    
    public void Fire(EnemyNavAgent target)
    {
        var lookPos = target.transform.position;
        weaponHolder.transform.DOLookAt(lookPos, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            if(target == null)return;
            var currentIndex = _nextFirePointIndex % firePoints.Length;

            var firePoint = firePoints[currentIndex];
            var projectile = Instantiate(_data.projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Projectile>();

            if (target != null)
            {
                projectile.SetTarget(target.transform,_data.attackPower);
            }

            Recoil(currentIndex);

            _nextFirePointIndex++; 
            
            //Particle
            if (_data.attackEffectPrefab != null)
            {
                Instantiate(_data.attackEffectPrefab, firePoint.position, firePoint.rotation);
            }

        });
        
    }
    
    private void Recoil(int index)
    {
        if (index < 0 || index >= weaponBarrels.Length) return;

        weaponHolder.transform.DOPunchScale(new Vector3(.125f, .125f, .125f), 0.35f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            weaponHolder.DOScale(Vector3.one, 1f);
        });

        DOTween.Kill(weaponBarrels[index]);

        weaponBarrels[index].DOPunchPosition(
            new Vector3(0, 0, -0.5f), 
            0.4f,                     
            1,                     
            0.5f           
        ).SetEase(Ease.OutQuad).SetId(weaponBarrels[index]);
    }
    

}