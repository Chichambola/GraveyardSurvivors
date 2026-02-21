using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
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

    private void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(RegenerationRoutine());
    }

    private void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
    }

    private void OnStatsChanged(CharacterStats stats)
    {
        _healthRegenerationValue = stats.HealthRegeneration;
    }

    public void SetInitialStats(float health, float healthRegenerationRate)
    {
        _maxValue = health;
        _currentValue = health;
        _healthRegenerationValue = healthRegenerationRate;
    }

    public void TakeDamage(float damage)
    {
        _currentValue -= damage;
        
        ValueChanged?.Invoke(_currentValue);
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
