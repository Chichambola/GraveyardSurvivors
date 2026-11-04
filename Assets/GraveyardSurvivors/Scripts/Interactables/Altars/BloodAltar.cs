using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using PrimeTween;
using TMPro.EditorUtilities;
using UnityEngine;

public class BloodAltar : Interactable
{
    [SerializeField] private CooldownHandler _cooldownHandler;
    [SerializeField] private List<float> _damagePercent;
    
    [Header("Blood drop animation")]
    [SerializeField] private Transform _startPosition;
    [SerializeField] private SpriteRenderer _bloodSprite;
    [SerializeField] private float _yOffset = 3f;
    [SerializeField] private float _dropDuration = 1f;
    [SerializeField] private Ease _dropEase;
    [Header("Blood animation")]
    [SerializeField] private GameObject _blood;
    [SerializeField] private float _scaleDuration = 1f;
    [SerializeField] private Ease _scaleEase;
    
    private const float MaxScale = 1f;
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
        
        _initialScale = MaxScale / _damagePercentDict.Count;
        
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
        StartAnimation();

        SetActive(false);

        _isOnCooldown = true;
        _cooldownHandler.TimePassed += OnTimePassed;
        _cooldownHandler.StartCountdown();
    }

    private void StartAnimation()
    {
        _bloodSprite.transform.position = _startPosition.position;

        float targetScale = _initialScale;
        
        if (_currentInteractionsAmount != 0)
            targetScale = _initialScale * _currentInteractionsAmount;
        
        Sequence.Create()
            .Group(Tween.LocalPositionY(_bloodSprite.transform, _bloodSprite.transform.position.y - _yOffset, _dropDuration))
            .Group(Tween.Scale(_blood.transform, targetScale, _scaleDuration));
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
