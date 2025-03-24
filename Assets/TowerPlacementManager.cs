using UnityEngine;
using Zenject;

public class TowerPlacementManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private LayerMask placementMask;
    [SerializeField] private float placementDuration = 5f;

    private bool canPlace = false;
    private TowerData currentTowerData;
    private DiContainer _container;

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    public void EnablePlacement(TowerData towerData)
    {
        canPlace = true;
        currentTowerData = towerData;
        Invoke(nameof(DisablePlacement), placementDuration);
    }

    private void DisablePlacement()
    {
        canPlace = false;
        currentTowerData = null;
    }

    private void Update()
    {
        if (!canPlace || Input.GetMouseButtonDown(1)) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementMask))
            {
                var slot = hit.transform;

                if (slot.childCount == 0)
                {
                    var tower = _container.InstantiatePrefab(towerPrefab, slot.position, Quaternion.identity, slot);
                    var towerComp = tower.GetComponent<Tower>();
                    towerComp.Initialize(currentTowerData);
                }
            }
        }
    }
}