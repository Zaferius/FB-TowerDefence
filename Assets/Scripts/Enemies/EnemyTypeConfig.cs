
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDefinition", menuName = "TD/Enemy Definition")]
public class EnemyDefinition : ScriptableObject
{
    public EnemyType type;

    public enum EnemyType
    {
        Runner,
        Attacker,
        Test
    }

    [Header("Stats")]
    public float health;
    public float speed;
    public int attackPower;
    public float attackCooldown;
    public float attackRange;

    [Header("Prefab")]
    public GameObject prefab;
}