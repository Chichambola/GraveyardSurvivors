using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro.EditorUtilities;
using UnityEngine;

public class HealthRegenerator : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _cooldown;

    public event Action<float> HealthRegenerated;
    
    private Coroutine _coroutine;
    private float _healthRegeneration;
    
    private void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Healing());
    }

    private void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
    }

    private void OnStatsChanged(CharacterStats stats)
    {
        _healthRegeneration = stats.HealthRegeneration;
    }

    private IEnumerator Healing()
    {
        var wait = new WaitForSecondsRealtime(_cooldown);

        while (enabled)
        {
            HealthRegenerated?.Invoke(_healthRegeneration);
            
            yield return wait;
        }
    }
}
