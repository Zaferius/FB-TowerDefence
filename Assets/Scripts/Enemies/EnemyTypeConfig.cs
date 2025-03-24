using ScriptableObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypeConfig", menuName = "TD/Enemy Type Config")]
public class EnemyTypeConfig : ScriptableObject
{
    public EnemyData.EnemyType type;
    public GameObject prefab;
}