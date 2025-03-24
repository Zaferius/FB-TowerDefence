using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemyData runnerData;
    [SerializeField] private EnemyData attackerData;
    [SerializeField] private Transform target;

    private void Start()
    {
        var e1 = Instantiate(enemyPrefab, new Vector3(0, 0, -5), Quaternion.identity);
        e1.GetComponent<Enemy>().Initialize(runnerData, target);

        var e2 = Instantiate(enemyPrefab, new Vector3(2, 0, -5), Quaternion.identity);
        e2.GetComponent<Enemy>().Initialize(attackerData, target);
    }
}
