using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PrimeTween;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class BloodAltar : Interactable
{
    [SerializeField] private CooldownHandler _cooldownHandler;
    [SerializeField] private List<float> _damagePercent;
    
    [Header("Blood drop animation")]
    [SerializeField] private Transform _startPosition;
    [SerializeField] private SpriteRenderer _bloodSprite;
    [SerializeField] private float _yOffset = 3f;
    [SerializeField] private TweenSettings<float> _settings;
    [SerializeField] private WaterAnimationHandler _animationHandler;
    
    private const float InitialYOffset = 99999f;
    private Dictionary<int,float> _damagePercentDict;
    private bool _isOnCooldown;
    private float _initialScale;
    private int _maxInteractionsAmount;
    private int _currentInteractionsAmount;

    private void OnEnable()
    {
        _isOnCooldown = false;
        
        _bloodSprite.transform.position = new Vector3(_bloodSprite.transform.position.x, InitialYOffset, _bloodSprite.transform.position.z);
        
        FillDictionary();
        
        _animationHandler.Init(_damagePercentDict.Count);
    }

    private void OnDisable()
    {
        if (_isOnCooldown)
            _cooldownHandler.TimePassed -= OnTimePassed;
    }

    private void FillDictionary()
    {
        _damagePercentDict = new Dictionary<int,float>();
        _maxInteractionsAmount = 0;
        
        foreach (var damagePercent in _damagePercent)
        {
            _damagePercentDict.Add(_maxInteractionsAmount, damagePercent);
            
            _maxInteractionsAmount++;
        }
        
        if (_damagePercentDict.Count == 0)
            throw new Exception();
        
        float value = _damagePercentDict[_currentInteractionsAmount];             
            
        Set(value);
    }

    public override void ResetCharacteristics()
    {
        _maxInteractionsAmount = 0;
        _currentInteractionsAmount = 0;
        IsAvailable = true;
    }
    
    public float GetDamagePercent() => _damagePercentDict[_currentInteractionsAmount];

    public void IncreaseInteractionAmount()
    {
        _currentInteractionsAmount ++;

        if (_currentInteractionsAmount >= _maxInteractionsAmount)
        {
            IsAvailable = false;
        }
        else
        {
            float value = _damagePercentDict[_currentInteractionsAmount];             
            
            Set(value);
        }
    }

    public void StartCountdown()
    {
        Tween tween = Tween.LocalPositionY(_bloodSprite.transform, _settings);
        
        _animationHandler.StartAnimation(_currentInteractionsAmount, tween);

        SetActive(false);

        _isOnCooldown = true;
        _cooldownHandler.TimePassed += OnTimePassed;
        _cooldownHandler.StartCountdown();
    }

    private void OnTimePassed()
    {
        if (_currentInteractionsAmount != _maxInteractionsAmount)
            SetActive(true);
        
        _isOnCooldown = false;
        
        _cooldownHandler.TimePassed -= OnTimePassed;
    }
    
    private void SetActive(bool isActive)
    {
        SetVisibility(isActive);
        IsAvailable = isActive;
    }
}