using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveEntry
{
    public EnemyDefinition enemyDefinition;
    public int count;
}

[CreateAssetMenu(menuName = "Waves/WaveData")]
public class WaveData : ScriptableObject
{
    public List<WaveEntry> entries;
    
    public int TotalEnemyCount
    {
        get
        {
            var total = 0;
            foreach (var entry in entries)
            {
                total += entry.count;
            }
            return total;
        }
    }
}
