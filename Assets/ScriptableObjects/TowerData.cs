using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TD/Tower Data", order = 1)]
public class TowerData : ScriptableObject
{
    public enum TowerType
    {
        Single,
        DoubleBarrel
    }
    
    public TowerType type;
    public GameObject prefab;
    [Space(10)]
    public float health;
    public float range;
    public float attackTimer;
    public int damage;
}
