using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private LanternLight _light;
    [SerializeField] private float _shrinkRateIncrease = 0.05f;
    [Header("Services")]
    [SerializeField] private LanternDamageDealer _damageDealer;
    [Header("Player")]
    [SerializeField] private Player _player;

    private List<Enemy> _enemiesInRange;
    private float _lastRadius;
    private float _targetValue = 0;
    private Coroutine _coroutine;

    private void Awake()
    {
        _enemiesInRange = new List<Enemy>();
    }

    private void OnEnable()
    {
        _player.EnemyWasKilled += OnEnemyDeath;
        _light.ThresholdReached += OnThresholdReached;
        _damageDealer.EnemyDetected += OnEnemyDetected;
        _damageDealer.EnemyLeft += OnEnemyLeft;
        _light.GainedEnergy += OnEnergyGained;
    }
    
    private void OnDisable()
    {
        _player.EnemyWasKilled -= OnEnemyDeath;
        _damageDealer.EnemyDetected -= OnEnemyDetected;
        _damageDealer.EnemyLeft -= OnEnemyLeft;
        _light.ThresholdReached -= OnThresholdReached;
        _light.GainedEnergy -= OnEnergyGained;
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        if(enemy == null)
            throw new Exception("Enemy cannot be null!");
        
        if (_light.gameObject.activeSelf)
        {
            _light.ReceiveEnergy(enemy.CurrentStats.LanternEnergy);
        }
        else
        {
            float tempValue = UserUtils.AddPercentToNumber(_lastRadius, enemy.CurrentStats.LanternEnergy);

            _light.gameObject.SetActive(true);
                
            _light.SetLightRadiusForAllAxis(tempValue);
                
            _light.StartRadiusRoutine(_targetValue);
        }
    }
    
    private void OnThresholdReached()
    {
        _lastRadius = _light.CurrentRadius;

        _light.gameObject.SetActive(false);
    }
    
    private void OnEnemyLeft()
    {
        DecreaseRate();
    }
    
    private void DecreaseRate()
    {
        float currentRate = _light.ShrinkRate;
        
        currentRate -= _shrinkRateIncrease;

        if (currentRate < 0)
            throw new Exception("Shrinking rate can't be less than 0");
        
        _light.SetRate(currentRate);
    }
    
    private void OnEnemyDetected()
    {
        IncreaseRate();
    }

    private void IncreaseRate()
    {
        float currentRate = _light.ShrinkRate;
        
        currentRate += _shrinkRateIncrease;
        
        _light.SetRate(currentRate);
    }
    
    private void OnEnergyGained() => _light.StartRadiusRoutine(_targetValue);
}
