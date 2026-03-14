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
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private LanternDamageDealer _damageDealer;
    [Header("Player")]
    [SerializeField] private Player _player;

    private List<Enemy> _enemiesInRange;
    private float _lastRadius;
    private Coroutine _coroutine;

    private void Awake()
    {
        _enemiesInRange = new List<Enemy>();
    }

    private void OnEnable()
    {
        _player.EnemyWasKilled += OnEnemyDeath;
        _enemyDetector.EnemyDetected += OnEnemyDetected;
        _enemyDetector.EnemyLeft += OnEnemyLeft;
        _light.ThresholdReached += OnThresholdReached;
    }

    private void OnDisable()
    {
        _player.EnemyWasKilled -= OnEnemyDeath;
        _enemyDetector.EnemyDetected -= OnEnemyDetected;
        _enemyDetector.EnemyLeft -= OnEnemyLeft;
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
                
                _light.SetLightRadiusForAllAxis(tempValue);
            }
            else
            {
                _lastRadius = tempValue;
            }
        }
    }
    
    private void OnThresholdReached()
    {
        _lastRadius = _light.CurrentRadius;

        _light.gameObject.SetActive(false);
    }
    
    private void OnEnemyLeft(Enemy enemy)
    {
        enemy.CanBeReleased -= OnEnemyLeft;
        
        if (_enemiesInRange.Contains(enemy))
        {
            _enemiesInRange.Remove(enemy);  
            
            _damageDealer.UpdateEnemies(_enemiesInRange.ToList());
            
            DecreaseRate();
        }
    }
    private void DecreaseRate()
    {
        float currentRate = _light.ShrinkRate;
        
        currentRate -= _shrinkRateIncrease;

        if (currentRate < 0)
            throw new Exception("Shrinking rate can't be less than 0");
        
        _light.SetRate(currentRate);
    }
    
    private void OnEnemyDetected(Enemy enemy)
    {
        enemy.CanBeReleased += OnEnemyLeft;
        
        _enemiesInRange.Add(enemy); 
        
        _damageDealer.UpdateEnemies(_enemiesInRange.ToList());
        
        IncreaseRate();
    }

    private void IncreaseRate()
    {
        float currentRate = _light.ShrinkRate;
        
        currentRate += _shrinkRateIncrease;
        
        _light.SetRate(currentRate);
    }
}
