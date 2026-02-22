using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Health : Stats<CharacterStats>
{
    [SerializeField] private Player _player;
    [SerializeField] private float _cooldownRate = 1f;

    public event Action<float> ValueChanged;
    
    private float _maxValue;
    private float _currentValue;
    private float _healthRegenerationValue;
    private Coroutine _coroutine;

    public float MaxHealth => _maxValue;
    public float CurrentValue => _currentValue;

    protected override void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(RegenerationRoutine());
    }

    protected override void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
    }

    public void TakeDamage(float damage)
    {
        _currentValue -= damage;
        
        ValueChanged?.Invoke(_currentValue);
    }

    protected override void OnStatsChanged(CharacterStats stats)
    {
        _currentValue = stats.Health;
        _healthRegenerationValue = stats.HealthRegeneration;
        
        if(_currentValue >= _maxValue)
            _maxValue = _currentValue;
    }

    public override void SetInitialStats(CharacterStats stats)
    {
        _currentValue = stats.Health;
        _maxValue = _currentValue;
        _healthRegenerationValue = stats.HealthRegeneration;
    }

    private IEnumerator RegenerationRoutine()
    {
        var wait = new WaitForSecondsRealtime(_cooldownRate);

        while (enabled)
        {
            _currentValue += _healthRegenerationValue;
            
            if (_currentValue >= _maxValue)
                _currentValue = _maxValue;
            
            ValueChanged?.Invoke(_currentValue);
            
            yield return wait;
        }
    }
    
    
}
