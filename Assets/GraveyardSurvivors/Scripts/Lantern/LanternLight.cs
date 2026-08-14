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
    [FormerlySerializedAs("thresholdValidator")] [FormerlySerializedAs("_thresholdVerifier")] [SerializeField] private ThresholdValidator _thresholdValidator;
    [Header("Radius values")]
    [SerializeField] private float _disableThreshold = 0.3f;
    [SerializeField] private float _shrinkTime = 25f;
    [SerializeField] private float _rateWhenGainingEnergy = 1f;
    [SerializeField] private float _maxTimeScale = 2;
    
    private float _lastRadius;
    private float _initialRange;
    private readonly int _defaultValue = 0;
    private readonly float _defaultTimeScale = 1;
    private bool _isRadiusActive;

    public bool IsActive => _radius.Value > _disableThreshold;
    public Vector3 CurrentPosition => transform.position;

    public void Init()
    {
        if (!(_radius.Value > 0))
            return;
        
        _radius.ChangeRadius(_defaultValue, _shrinkTime);
        _thresholdValidator.Execute(_radius, _disableThreshold);
        _isRadiusActive = true;
        _thresholdValidator.ThresholdReached += OnThresholdReached;
    }

    private void Awake()
    {
        _lastRadius = _disableThreshold;
    }

    private void OnDisable()
    {
        _thresholdValidator.ThresholdReached -= OnThresholdReached;
    }

    public void ProcessEnemyDeath(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception("Enemy cannot be null!");
        
        if (_isRadiusActive)
        {
            float targetRadius = _radius.Value.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);
            
            _radius.ChangeRadius(targetRadius, _rateWhenGainingEnergy);
        }
        else
        {
            _lastRadius = _lastRadius.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);

            if (!(_lastRadius > _disableThreshold))
                return;

            _isRadiusActive = true;
            
            _radius.SetActive(true);
            
            StartLight();
            
            _thresholdValidator.Execute(_radius, _disableThreshold);
        }
    }

    public void ResetRadius()
    {
        _light.range = _initialRange;
        
        _radius.ChangeRadius(_radius.InitialValue, _rateWhenGainingEnergy);
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

    public void StartLight()
    {
        float remainingTime = (_radius.Value / _radius.InitialValue) * _shrinkTime;
        
        _radius.ChangeRadius(_defaultValue, remainingTime);
    }
    
    private void OnThresholdReached()
    {
        _isRadiusActive = false;
        _radius.SetActive(false);
        _radius.StopChanging();
        _thresholdValidator.StopValidating();
    }
}