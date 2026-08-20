using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Serialization;

public class LanternLight : MonoBehaviour, ILantern
{
    [Header("Visuals")]
    [SerializeField] private Light _light;
    [SerializeField] private RadiusEffectScaler _radius;
    [SerializeField] private ThresholdValidator _thresholdValidator;
    [Header("Radius values")]
    [SerializeField] private float _disableThreshold = 0.3f;
    [SerializeField] private float _shrinkTime = 25f;
    [SerializeField] private float _rateWhenGainingEnergy = 1f;
    [SerializeField] private float _maxTimeScale = 2;
    
    private float _lastRadius;
    private bool _isSubscribed;
    private readonly int _defaultValue = 0;
    private readonly float _defaultTimeScale = 1;

    public bool IsActive => _radius.Value > _disableThreshold;
    public Vector3 CurrentPosition => transform.position;

    private void Awake()
    {
        _lastRadius = _disableThreshold;
    }
    
    private void OnEnable()
    {
        _thresholdValidator.ThresholdReached += OnThresholdReached;
    }

    private void OnDisable()
    {
        _thresholdValidator.ThresholdReached -= OnThresholdReached;
    }

    private void Start()
    {
        _radius.ChangeRadius(_defaultValue, _shrinkTime);
        _thresholdValidator.Execute(_radius, _disableThreshold);
    }

    public void ProcessEnemyDeath(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception("Enemy cannot be null!");
        
        if (_radius.IsActive)
        {
            if (_isSubscribed)
                _radius.Reached -= OnRadiusReached;
            
            float targetRadius = _radius.Value.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);
            
            _radius.ChangeRadius(targetRadius, _rateWhenGainingEnergy);
            
            _radius.Reached += OnRadiusReached;
            
            _isSubscribed = true;
        }
        else
        {
            _lastRadius = _lastRadius.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);

            if (!(_lastRadius > _disableThreshold))
                return;
            
            _radius.SetActive(true);
            
            _radius.ChangeRadius(_lastRadius, _rateWhenGainingEnergy);
            
            _thresholdValidator.Execute(_radius, _disableThreshold);
        }
    }

    public void ResetRadius(float speed)
    {
        if (_radius.IsEqualToInitialValue) 
            return;
        
        if (!_radius.IsActive)
            _radius.SetActive(true);
        
        if (!_light.enabled)
            _light.enabled = true;
        
        _radius.ChangeRadius(_radius.InitialValue, speed);
        _thresholdValidator.Execute(_radius, _disableThreshold);
    }

    public void ChangeSpeed(float multiplier, float factor)
    {
        float progress = (_radius.TimeScale - _defaultTimeScale) / (_maxTimeScale - _defaultTimeScale);

        float bonusMultiplier = Mathf.Max(multiplier / 100) * (1f - progress);

        bonusMultiplier = Mathf.Max(bonusMultiplier, 0);
        
        float speedBonus = (_maxTimeScale - _defaultTimeScale) * bonusMultiplier;
        
        float timeScale = _defaultTimeScale + (speedBonus * factor);

        timeScale = Mathf.Min(timeScale, _maxTimeScale);
        
        _radius.SetTimeScale(timeScale);
    }

    public void StartChanging()
    {
        float remainingTime = (_radius.Value / _radius.InitialValue) * _shrinkTime;
        
        _radius.ChangeRadius(_defaultValue, remainingTime);
    }

    public void StartLight() => _radius.Resume();

    public void PauseLight() => _radius.Pause();
    
    private void OnThresholdReached()
    {
        _radius.SetActive(false);
        _radius.StopChanging();
        _light.enabled = false;
        _thresholdValidator.StopValidating();
    }
    
    private void OnRadiusReached()
    {
        StartChanging();

        _isSubscribed = false;

        _lastRadius = _radius.Value;
        
        _radius.Reached -= OnRadiusReached;
    }
}