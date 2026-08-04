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
    [Header("Radius values")]
    [SerializeField] private float _disableThreshold = 0.3f;
    [SerializeField] private float _shrinkTime = 25f;
    [SerializeField] private float _currentDuration;
    
    private float _lastRadius;
    private float _initialRange;
    private int _defaultValue = 0;
    private bool _isRadiusActive;

    public bool IsActive => _radius.Value > _disableThreshold;
    public Vector3 CurrentPosition => transform.position;

    public void Init()
    {
        if (!(_radius.Value > 0))
            return;
        
        _radius.ThreshholdReached += OnThresholdReached;
        _radius.Init();
        _radius.ChangeRadius(_defaultValue, _shrinkTime).Forget();
        _isRadiusActive = true;
    }

    private void Awake()
    {
        _lastRadius = _disableThreshold;
    }

    private void OnDisable()
    {
        _radius.ThreshholdReached -= OnThresholdReached;
    }

    public async void ProcessEnemyDeath(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception("Enemy cannot be null!");
        
        if (_isRadiusActive)
        {
            float energyValue = _radius.Value.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);

            var timePercentage = UserUtils.CalculatePercentageOf(_radius.Value, _radius.InitialValue);

            var tempShrinkTime= _shrinkTime.GetClampedValueInverse(timePercentage);

            _currentDuration = _shrinkTime.SubtractPercentFromNumber(tempShrinkTime);

            _radius.ChangeRadius(_currentDuration).Forget();
        }
        else
        {
            _lastRadius = _lastRadius.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);

            if (!(_lastRadius > _disableThreshold))
                return;

            _isRadiusActive = true;
            
            _radius.SetActive(true);
            
            await _radius.ChangeRadius(_lastRadius);
        }
    }

    public void ResetRadius()
    {
        _light.range = _initialRange;
        
        _radius.ChangeRadius(_radius.InitialValue).Forget();
    }
    
    public void IncreaseSpeed(float duration) => _radius.IncreaseSpeed(duration);
    
    public void DecreaseSpeed(float duration) => _radius.DecreaseSpeed(duration);
    
    private void OnThresholdReached()
    {
        _isRadiusActive = false;
        _radius.SetActive(false);
    }
}