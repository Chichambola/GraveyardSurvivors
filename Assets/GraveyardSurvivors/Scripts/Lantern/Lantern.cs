using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] private LanternLight _light;
    [SerializeField] private LanternDamageDealer _damageDealer;
    [SerializeField] private Player _player;

    private float _lastRadius;
    private Coroutine _coroutine;
    
    private void OnEnable()
    {
        _player.EnemyWasKilled += OnEnemyDeath;
        _damageDealer.DamageDealt += OnDamageDealt;
        _light.ThresholdReached += OnThresholdReached;
    }

    private void OnDisable()
    {
        _player.EnemyWasKilled -= OnEnemyDeath;
        _damageDealer.DamageDealt -= OnDamageDealt;
        _light.ThresholdReached -= OnThresholdReached;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        if (_light.gameObject.activeSelf)
        {
            _light.ReceiveEnergy(enemy.CurrentStats.LanternEnergy);
        }
        else
        {
            float tempValue = UserUtils.AddPercentToNumber(_lastRadius, enemy.CurrentStats.LanternEnergy);
            
            if (tempValue >= _lastRadius)
            {
                _light.gameObject.SetActive(true);
                
                _light.SetRadius(tempValue);
            }
            else
            {
                _lastRadius = tempValue;
            }
        }
    }
    
    private void OnDamageDealt(float damagePercent)
    {
        float currentRadius = _light.CurrentRadius;

        currentRadius = UserUtils.SubtractPercentFromNumber(currentRadius, damagePercent);
        
        _light.SetRadius(currentRadius);
    }
    
    private void OnThresholdReached()
    {
        _lastRadius = _light.CurrentRadius;

        _light.gameObject.SetActive(false);
    }
}
