using System;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
     public HealthBarController healthBarPrefab;

    private HealthBarController _instance;
    private IHealth _health;
    private float _visibleTimer = -1f;

    private void Awake()
    {
        _health = GetComponent<IHealth>();
    }
    
    private void Start()
    {
        if (_health != null && healthBarPrefab != null)
        {
            _instance = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity, transform);
            _instance.transform.localPosition = Vector3.up * 2.5f;
            _instance.SetVisible(false);
            
            _health.OnHealthChanged += OnHealthChanged;
        }
    }

    private void Update()
    {
        if (_visibleTimer > 0f)
        {
            _visibleTimer -= Time.deltaTime;

            if (_visibleTimer <= 0f && _instance != null)
            {
                _instance.SetVisible(false);
            }
        }
    }

    private void OnHealthChanged(float current, float max)
    {
        if (_instance == null) return;

        _instance.SetHealth(current, max);
        _instance.SetVisible(true);
        _visibleTimer = 6f;
        
    }

    private void OnDestroy()
    {
        if (_health != null)
            _health.OnHealthChanged -= OnHealthChanged;
    }
}