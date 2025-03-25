using UnityEngine;

public class TowerSelectionPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot; // panelin aktif/deaktif edileceği root

    private bool _isOpen;

    public bool IsOpen => _isOpen;

    public void Show()
    {
        _isOpen = true;
        panelRoot.SetActive(true);
    }

    public void Hide()
    {
        _isOpen = false;
        panelRoot.SetActive(false);
    }
}