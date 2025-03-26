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

    [Header("Appearance")]
    public GameObject attackEffectPrefab; 
    public Color towerColor = Color.white;
    
    [Header("Prefab")]
    public GameObject prefab;
    public GameObject projectilePrefab;
}
