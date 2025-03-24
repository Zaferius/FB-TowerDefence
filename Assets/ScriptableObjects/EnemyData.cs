using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "TD/Enemy Data", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public float health;
        public float speed;
        public EnemyType type;

        public enum EnemyType
        {
            Runner,
            Attacker,
            Test
        }
    }
}