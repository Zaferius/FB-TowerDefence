using DG.Tweening;
using UnityEngine;
using Zenject;

public class TowerFactory : IFactory<Vector3, Transform, Tower>
{
    private readonly GameObject _prefab;
    private readonly DiContainer _container;

    public TowerFactory(GameObject prefab, DiContainer container)
    {
        _prefab = prefab;
        _container = container;
    }

    public Tower Create(Vector3 position, Transform parent)
    {
        Debug.Log("Creating Tower");
        var lastPos = new Vector3(position.x, _prefab.transform.localScale.y, position.z);
        var obj = _container.InstantiatePrefab(_prefab, lastPos, Quaternion.identity, parent);

        var objScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;
        obj.transform.DOScale(objScale, 0.1f).SetEase(Ease.OutBack);
        
        return obj.GetComponent<Tower>();
    }
}