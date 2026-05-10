using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Serialization;

public class LanternLight : MonoBehaviour
{
    [Header("Visuals and collider")]
    [SerializeField] private ParticleSystem _lightArea;
    [SerializeField] private Light _light;
    [SerializeField] private SphereCollider _collider;
    [Header("Radius values")]
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _disableThreshold = 0.3f;
    [SerializeField] private float _shrinkRate = 0.1f;

    public event Action<LanternLight> GainedEnergy;
    public event Action<LanternLight> ThresholdReached;
    
    private Coroutine _coroutine;
    private float _initialRadius;
    private float _energyMultiplier = 1.5f;
    private float _initialRate;
    private float _lastRadius;
    private float _initialRange;
    private float _targetRadius;
    private int _defaultValue = 0;

    public bool IsActive => _collider.radius > _disableThreshold;
    public float ShrinkRate => _shrinkRate;
    public float CurrentRadius => _collider.radius;
    private bool IsGainingEnergy => _targetRadius != 0;

    public void Init()
    {
        if (_radius > 0)
        {
            SetLightRadius(_radius);
            StartRadiusRoutine();
        }
    }
    
    private void Awake()
    {
        _initialRadius = _radius;
        _initialRate = _shrinkRate;
        _initialRange = _light.range;
        _lastRadius = _disableThreshold;
    }

    public void SetRate(float value) => _shrinkRate = value;
    
    public void ResetRate() => _shrinkRate = _initialRate;
    
    public void StartRadiusRoutine(float targetValue = 0f)
    {
        _targetRadius = targetValue;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangingRadiusRoutine());
    }

    public void ProcessEnemyDeath(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception("Enemy cannot be null!");

        if (_collider.radius > _disableThreshold)
        {
            float tempRadius = _collider.radius.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);

            if (tempRadius > _initialRadius)
            {
                _targetRadius = _defaultValue;
            }
            else
            {
                _targetRadius = tempRadius;
            }
        }
        else
        {
            _lastRadius = _lastRadius.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);

            if (_lastRadius > _disableThreshold)
            {
                _targetRadius = _lastRadius;

                _lastRadius = _disableThreshold;
            }
        }
    }

    public void ResetRadius()
    {
        _light.range = _initialRange;
        
        StartRadiusRoutine(_radius);
    }

    private float LerpToValue(float currentValue, float finalValue)
    {
        float value;
        
        if (IsGainingEnergy)
        {
            if (Mathf.Approximately(_collider.radius, finalValue))
            {
                GainedEnergy?.Invoke(this);

                _targetRadius = _defaultValue;
            }
            
            value = Mathf.MoveTowards(currentValue, finalValue, Time.fixedDeltaTime);

            return value;
        }

        value = Mathf.MoveTowards(currentValue, finalValue, _shrinkRate * Time.fixedDeltaTime);

        return value;
    }
    
    private IEnumerator ChangingRadiusRoutine()
    {
        while (enabled) 
        {
            _collider.radius = LerpToValue(_collider.radius, _targetRadius);

            _light.range = LerpToValue(_light.range, _targetRadius);

            SetLightRadius(_collider.radius);

            if (_collider.radius <= _disableThreshold && IsGainingEnergy == false)
            {
                ThresholdReached?.Invoke(this);
                
                SetLightRadius(_defaultValue);
            }
            
            yield return null;
        }
    }
    
    private void SetLightRadius(float value)
    {
        var particleSize = new Vector3(value, value, value);

        _lightArea.transform.localScale = particleSize;
        _collider.radius = value;
        _light.range = value;
    }
}