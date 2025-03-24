using UnityEngine;
using Zenject;

public class TowerPlacementManager : MonoBehaviour
{
    [Inject] private TowerFactory _towerFactory;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask placementMask;
    [SerializeField] private float placementDuration = 5f;
    [SerializeField] private TowerData towerData;

    private bool canPlace = false;

    private void Start()
    {
        EnablePlacement(towerData); // for testt
    }

    public void EnablePlacement(TowerData data)
    {
        towerData = data;
        canPlace = true;
        Invoke(nameof(DisablePlacement), placementDuration);
    }

    private void DisablePlacement()
    {
        canPlace = false;
    }

    private void Update()
    {
        if (!canPlace || Input.GetMouseButtonDown(1)) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementMask))
            {
                Transform slot = hit.transform;

                if (slot.childCount == 0)
                {
                    var tower = _towerFactory.Create(slot.position, slot);
                    tower.Initialize(towerData);
                }
            }
        }
    }
}