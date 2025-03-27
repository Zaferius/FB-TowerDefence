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
}
