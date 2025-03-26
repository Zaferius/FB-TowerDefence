using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyNavAgent>() != null)
        {
            var enemy = other.gameObject.GetComponent<EnemyNavAgent>();
            Time.timeScale = 0f;
            WaveManager.OnGameOver?.Invoke();
        }
    }
}
