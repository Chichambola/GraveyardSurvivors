using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Serialization;

public class LanternLight : MonoBehaviour
{
    [SerializeField] private ParticleSystem _lightArea;
    [SerializeField] private Light _light;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _disableThreshold = 0.3f;
    [SerializeField] private float _shrinkRate = 0.1f;

    public event Action<LanternLight> ThresholdReached;
    public event Action<LanternLight> GainedEnergy;

    private Coroutine _coroutine;
    private float _initialRadius;
    private float _energyMultiplier = 1.5f;
    private int _defaultValue = 0;
    private bool _isGainingEnergy;
    private float _initialRate;

    public float CurrentRadius => _collider.radius;
    public float ShrinkRate => _shrinkRate;
    public bool IsActive => _collider.radius > _disableThreshold;
    public bool IsGainingEnergy => _isGainingEnergy;

    public void Init()
    {
        if (_radius > 0)
        {
            SetLightRadiusForAllAxis(_initialRadius);
            StartRadiusRoutine(_defaultValue);   
        }
    }
    
    private void Awake()
    {
        _initialRadius = _radius;
        _initialRate = _shrinkRate;
    }
    
    public void ReceiveEnergy(float energyAmount)
    {
        float value = UserUtils.AddPercentToNumber(_collider.radius, energyAmount);

        SetGainingEnergyState(true);

        StartRadiusRoutine(value);
    }

    public void SetLightRadiusForAllAxis(float value)
    {
        var particleSize = new Vector3(value, value, value);

        _lightArea.transform.localScale = particleSize;
        _collider.radius = value;
    }

    public void ResetRadius()
    {
        SetGainingEnergyState(true);
        
        StartRadiusRoutine(_initialRadius);
    }

    public void SetRate(float value) => _shrinkRate = value;

    public void ResetRate() => _shrinkRate = _initialRate;

    public void SetGainingEnergyState(bool value) => _isGainingEnergy = value;
    
    public void StartRadiusRoutine(float targetValue)
    {
        if (gameObject.activeSelf)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ChangingRadiusRoutine(targetValue));
        }
    }

    public void ChangeState(bool isShrinking)
    {
        if (isShrinking)
        {
            StartRadiusRoutine(_defaultValue);
        }
        else
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);   
            
            SetLightRadiusForAllAxis(_defaultValue);

            _light.range = _defaultValue;
        }
    }

    private IEnumerator ChangingRadiusRoutine(float targetValue)
    {
        while (enabled)
        {
            _collider.radius = LerpToValue(_collider.radius, targetValue);

            _light.range = LerpToValue(_light.range, targetValue);

            SetLightRadiusForAllAxis(_collider.radius);
            
            if (_collider.radius <= _disableThreshold && _isGainingEnergy == false)
            {
                ChangeState(false);
                
                ThresholdReached?.Invoke(this);
            }
            
            yield return null;
        }
    }

    private float LerpToValue(float currentValue, float finalValue)
    {
        float value;
        
        if (_isGainingEnergy)
        {
            if (Mathf.Approximately(_collider.radius, finalValue))
            {
                SetGainingEnergyState(false);
                
                GainedEnergy?.Invoke(this);
            }
            
            value = Mathf.MoveTowards(currentValue, finalValue, Time.fixedDeltaTime * _energyMultiplier);

            return value;
        }

        value = Mathf.MoveTowards(currentValue, finalValue, _shrinkRate * Time.fixedDeltaTime);

        return value;
    }
}