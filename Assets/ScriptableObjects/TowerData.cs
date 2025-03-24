using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "TD/Tower Data", order = 1)]
public class TowerData : ScriptableObject
{
    public float health;
    public float range;
    public float fireRate;
    public int damage;
}
