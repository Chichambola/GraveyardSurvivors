using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternHealer : MonoBehaviour
{
    [SerializeField] private PlayerDetector _detector;
    [SerializeField] private float _buffAmount = 1.5f;
    [SerializeField] private float _cooldown = 1.5f;
    
    private float _healthRegenerationAmount;
    private Player _player;
    private Coroutine _coroutine;
    
    private void OnEnable()
    {
        _detector.PlayerDetected += StartHealing;
        _detector.PlayerLeft += StopHealing;
    }

    private void OnDisable()
    {
        _detector.PlayerDetected -= StartHealing;
        _detector.PlayerLeft -= StopHealing;
    }

    private void StartHealing(Player player)
    {
        _player = player;
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(HealingCoroutine());
    }

    private void StopHealing(Player player)
    {
        _player.CurrentStats.HealthRegeneration -= _healthRegenerationAmount;

        _healthRegenerationAmount = 0;
        
        StopCoroutine(_coroutine);

        if (_player == player)
        {
            _player = null;
        }
        else
        {
            throw new Exception();
        }
    }
    
    private IEnumerator HealingCoroutine()
    {
        var wait = new WaitForSecondsRealtime(_cooldown);

        while (enabled)
        {
            _healthRegenerationAmount += _buffAmount;

            _player.CurrentStats.HealthRegeneration += _buffAmount;
            
            yield return wait;
        }
    }
}
