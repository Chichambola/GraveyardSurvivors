using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private float _healIncrease = 1.5f;
    [SerializeField] private float _cooldown = 1.5f;
    
    private Player _player;
    private float _initialHealthRegeneration;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _playerDetector.PlayerDetected += StartHealing;
        _playerDetector.PlayerLeft += StopHealing;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerDetected -= StartHealing;
        _playerDetector.PlayerLeft -= StopHealing;
    }

    private void StartHealing(Player player)
    {
        _initialHealthRegeneration = player.CurrentStats.HealthRegeneration;
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(HealingCoroutine());
    }

    private void StopHealing()
    {
        _player.CurrentStats.HealthRegeneration = _initialHealthRegeneration;
        
        StopCoroutine(_coroutine);
    }
    
    private IEnumerator HealingCoroutine()
    {
        var wait = new WaitForSecondsRealtime(_cooldown);

        while (enabled)
        {
            _player.CurrentStats.HealthRegeneration += _healIncrease;
            
            yield return wait;
        }
    }
}
