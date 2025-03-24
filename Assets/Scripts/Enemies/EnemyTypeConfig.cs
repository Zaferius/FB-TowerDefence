using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "TD/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    public EnemyData.EnemyType type;

    [Header("Stats")]
    public float health;
    public float speed;
    public float attackCooldown;
    public float attackRange;

    [Header("Prefab")]
    public GameObject prefab;
}