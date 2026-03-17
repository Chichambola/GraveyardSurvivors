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
    public event Action GainedEnergy;
    
    private SphereCollider _collider;
    private Coroutine _coroutine;
    private float _initialRadius;
    private float _energyMultiplier = 5f;
    private bool _isGainingEnergy;

    public float CurrentRadius => _collider.radius;
    public float ShrinkRate => _shrinkRate;
    
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        
        _initialRadius = _radius;
    }

    private void OnEnable()
    {
        StartRadiusRoutine(0);
    }

    private void Start()
    {
        SetLightRadiusForAllAxis(_initialRadius);
    }
    
    public void ReceiveEnergy(float energyAmount)
    {
        float value = UserUtils.AddPercentToNumber(_collider.radius, energyAmount);

        _isGainingEnergy = true;
        
        StartRadiusRoutine(value);
    }
    
    public void SetLightRadiusForAllAxis(float value)
    {
        var particleSize = new Vector3(value, value, value);

        _lightArea.transform.localScale = particleSize;
        _collider.radius = value;
    }

    public void ResetRadius() => StartRadiusRoutine(_initialRadius);
    
    public void SetRate(float value) => _shrinkRate = value;

    public void StartRadiusRoutine(float targetValue)
    {
        if (gameObject.activeSelf)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ChangingRadiusRoutine(targetValue));
        }
    }

    private IEnumerator ChangingRadiusRoutine(float targetValue)
    {
        while (enabled)
        {
            ChangeRadius(targetValue);   
            
            if (_collider.radius <= _disableThreshold)
            {
                ThresholdReached?.Invoke();
            }
            
            yield return null;
        }
    }

    private void ChangeRadius(float targetValue)
    {
        _collider.radius = LerpToValue(_collider.radius, targetValue);

        _light.range = LerpToValue(_light.range, targetValue);
            
        SetLightRadiusForAllAxis(_collider.radius);
    }
    
    private float LerpToValue(float currentValue, float finalValue)
    {
        float value;
        
        if (_isGainingEnergy)
        {
            value = Mathf.Lerp(currentValue, finalValue, Time.fixedDeltaTime * _energyMultiplier);
            
            if (Mathf.Approximately(_collider.radius, finalValue))
            {
                _isGainingEnergy = false;
                
                GainedEnergy?.Invoke();
            }
        }
        else
        {
            value = Mathf.Lerp(currentValue, finalValue, _shrinkRate * Time.fixedDeltaTime);
        }

        return value;
    }
}
