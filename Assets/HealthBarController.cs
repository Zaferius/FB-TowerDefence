using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Canvas canvas;
    private Transform _cameraTransform;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.forward = _cameraTransform.forward;
    }

    public void SetHealth(float current, float max)
    {
        slider.value = current / max;
    }

    public void SetVisible(bool visible)
    {
        canvas.enabled = visible;
    }
}