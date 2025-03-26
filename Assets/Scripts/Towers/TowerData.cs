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
    
    [Header("Stats")]
    public float health;
    public float range;
    public float attackTimer;
    public int attackPower;
    
    [Header("Prefab")]
    public GameObject prefab;
}
