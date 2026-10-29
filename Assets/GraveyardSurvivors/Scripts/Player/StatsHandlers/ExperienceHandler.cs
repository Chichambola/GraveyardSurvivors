using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = PrimeTween.Sequence;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private float _experienceIncreasePercent = 155;
    [SerializeField] private int _initialTargetXp = 50;
    [SerializeField] private int _maxXp = 5000;
    [SerializeField] private SliderHandler _slider;
    
    public event Action PlayerReachedThreshold; 
    
    private const int MinXp = 10;
    private const int MinCurrentXp = 0;
    private int _currentXp;
    private int _targetXp;
    private int _lastXp;

    private void Awake()
    {
        if (_initialTargetXp <= 0)
        {
            throw new Exception("Target experience can not be less or equal to 0");
        }
        
        _targetXp = _initialTargetXp;
        _currentXp = 0;
    }

    private void OnEnable()
    {
        _slider.Set(MinCurrentXp,_targetXp);
    }

    private void OnValidate()
    {
        if (_initialTargetXp <= MinXp)
        {
            _initialTargetXp = MinXp;
        }
    }

    public async UniTask GainExperience(int experience)
    {
        _currentXp += experience;
        
        if (_currentXp >= _targetXp)
        {
            PlayerReachedThreshold?.Invoke();
            
            await _slider.UpdateSlider(_targetXp);
            
            _currentXp -= _targetXp;

            var percent = _targetXp.GetClampedValue(_experienceIncreasePercent, _maxXp);

            _targetXp = _targetXp.AddPercentToNumber(percent);

            _slider.Set(MinCurrentXp,_targetXp);

            _slider.UpdateSlider(_currentXp).Forget();
        }
        else
        {
            await _slider.UpdateSlider(_currentXp);
        }
    }
}