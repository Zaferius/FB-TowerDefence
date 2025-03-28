using DG.Tweening;
using UnityEngine;

public class SingleBarrelFiring : MonoBehaviour, ITowerFiringStrategy
{
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform weaponBarrel;
    [SerializeField] private Transform firePoint;
    
    private TowerData _data;

    [SerializeField] private EnemyNavAgent _closestEnemy;

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
            
            var projectile = Instantiate(_data.projectilePrefab, firePoint.position, Quaternion.identity).GetComponent<Projectile>();
            
            if (target != null)
            {
                projectile.SetTarget(target.transform,_data.attackPower);
            }
            
            Recoil();
            
            //Particle test >>
            if (_data.attackEffectPrefab != null)
            {
                var effect = Instantiate(_data.attackEffectPrefab, firePoint.position, firePoint.rotation);
                Destroy(effect, 1.5f);
            }
            
        });
        
    }

    private void Recoil()
    {
        weaponHolder.transform.DOPunchScale(new Vector3(.125f, .125f, .125f), 0.35f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            weaponHolder.DOScale(Vector3.one, 1f);
        });
        
        DOTween.Kill(weaponBarrel);
        weaponBarrel.DOPunchPosition(
            new Vector3(0, 0, -0.5f), 
            0.4f,                     
            1,                     
            0.5f           
        ).SetEase(Ease.OutQuad).SetId(weaponBarrel);
    }
}