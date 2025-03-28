using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StickAttacker : MonoBehaviour,IAttackHandler
{
    [SerializeField] private Transform stick;
    [SerializeField] private int damage = 1;

    public void InitializeFromDefinition(EnemyDefinition definition)
    {
        damage = definition.attackPower;
    }

    public void DoAttack(Tower targetTower)
    {
        if (stick == null || targetTower == null) return;
        if (gameObject == null)
        {
            DOTween.Kill(gameObject);
            return;
        }

        transform.DOLookAt(targetTower.transform.position, .2f).SetEase(Ease.OutBounce).SetTarget(gameObject);
        
        stick.DOLocalRotate(new Vector3(75f, 0, 0), 0.15f)
            .SetEase(Ease.InBack)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
            .OnComplete(() =>
            {
                stick.DOLocalRotate(Vector3.zero, 0.3f)
                    .SetEase(Ease.OutBack)
                    .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

                if (targetTower.TryGetComponent<IHealth>(out var health))
                    health.TakeDamage(damage);
            });

    }

   
}
