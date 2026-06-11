using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LanternHealer : MonoBehaviour
{
    [SerializeField] private PlayerDetector _detector;
    [SerializeField] private float _healAmount = 1f;
    [SerializeField] private float _cooldown = 1.5f;
    
    private IPlayer _player;
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

    private void StartHealing(IPlayer player)
    {
        _player = player;
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(HealingCoroutine());
    }

    private void StopHealing()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);
    }
    
    private IEnumerator HealingCoroutine()
    {
        var wait = new WaitForSecondsRealtime(_cooldown);

        while (enabled)
        {
            _player.Heal(_healAmount);
            
            yield return wait;
        }
    }
}
