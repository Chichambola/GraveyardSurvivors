using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class LanternLight : MonoBehaviour
{
    [SerializeField] private ParticleSystem _lightArea;
    [SerializeField] private Light _light;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _disableThreshold = 0.3f;
    [SerializeField] private float _shrinkRate = 0.1f;

    public event Action ThresholdReached;
    
    private SphereCollider _collider;
    private Coroutine _coroutine;
    private float _initialRadius;

    public float CurrentRadius => _collider.radius;
    public float ShrinkRate => _shrinkRate;
    
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        
        _initialRadius = _radius;
    }

    private void OnEnable()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShrinkingCoroutine());
    }

    private void Start()
    {
        SetLightRadiusForAllAxis(_initialRadius);
    }
    
    public void ReceiveEnergy(float energyAmount) => _collider.radius = UserUtils.AddPercentToNumber(_collider.radius, energyAmount);

    public void ResetRadius() => SetLightRadiusForAllAxis(_initialRadius);
    
    public void SetLightRadiusForAllAxis(float value)
    {
        var particleSize = new Vector3(value, value, value);

        _lightArea.transform.localScale = particleSize;
        _collider.radius = value;
    }

    public void SetRate(float value)
    {
        _shrinkRate = value;
    }
    
    private IEnumerator ShrinkingCoroutine()
    {
        float finalValue = 0;
        
        while (enabled)
        {
            _collider.radius = DecreaseValue(_collider.radius, finalValue);

            _light.range = DecreaseValue(_light.range, finalValue);
            
            SetLightRadiusForAllAxis(_collider.radius);

            if (_collider.radius <= _disableThreshold)
            {
                ThresholdReached?.Invoke();
            }
            
            yield return null;
        }
    }

    private float DecreaseValue(float currentValue, float finalValue)
    {
        float value = Mathf.Lerp(currentValue, finalValue, _shrinkRate * Time.deltaTime);

        return value;
    }
}
