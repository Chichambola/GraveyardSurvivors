using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private float _experienceIncreasePercent = 155;
    [SerializeField] private float _initialTargetXp = 50;
    [SerializeField] private float _maxXp = 5000;
    [Header("Slider settings")]
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private Ease _ease = Ease.InOutSine;

    public event Action PlayerReachedThreshold; 
    
    private float _currentXp;
    private float _targetXp;
    private float _minXp = 10;
    private TweenSettings<float> _tweenSettings;

    private void Awake()
    {
        if (_initialTargetXp <= 0)
        {
            throw new Exception("Target experience can not be less or equal to 0");
        }
        
        _targetXp = _initialTargetXp;
        _currentXp = 0;
        
        _tweenSettings = new();
    }

    private void OnEnable()
    {
        _tweenSettings.settings.duration = _duration;
        _tweenSettings.settings.ease = _ease;
        _tweenSettings.settings.useUnscaledTime = true;
        
        ResetSliderValue();
    }

    private void OnValidate()
    {
        if (_initialTargetXp <= _minXp)
        {
            _initialTargetXp = _minXp;
        }
    }
    
    public void GainExperience(float experience)
    {
        float currentXp = _currentXp;
        
        _currentXp += experience;

        if (_currentXp >= _targetXp)
        {
            PlayerReachedThreshold?.Invoke();
            
            _currentXp -= _targetXp;

            var percent = _targetXp.GetClampedValue(_experienceIncreasePercent, _maxXp);

            _targetXp = _targetXp.AddPercentToNumber(percent).RoundToFifths();
            
            ResetSliderValue();
        }
        
        UpdateText(currentXp, _currentXp);
    }

    private void UpdateText(float current, float target)
    {
        _tweenSettings.startValue = current;
        _tweenSettings.endValue = target;
        
        Tween.UISliderValue(_slider, _tweenSettings);
    }

    private void ResetSliderValue()
    {
        _slider.maxValue = _targetXp;
        _slider.value = 0;
    }
}
