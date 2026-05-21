using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private float _experienceIncreasePercent = 155;
    [SerializeField] private float _initialTargetXp = 50;
    [SerializeField] private TextMeshProUGUI _text;

    public event Action PlayerReachedThreshold; 
    
    private float _currentXp;
    private float _targetXp;

    private void Awake()
    {
        if (_initialTargetXp <= 0)
        {
            throw new Exception("Target experience can not be less or equal to 0");
        }
        
        _targetXp = _initialTargetXp;
    }

    private void Start()
    {
        UpdateText();
    }

    public void GainExperience(float experience)
    {
        _currentXp += experience;

        if (_currentXp >= _targetXp)
        {
            PlayerReachedThreshold?.Invoke();
            
            _currentXp -= _targetXp;

            _targetXp = _targetXp.AddPercentToNumber(_experienceIncreasePercent).RoundToTenths();
        }
        
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = $"XP: {_currentXp} / {_targetXp}";
    }
}
