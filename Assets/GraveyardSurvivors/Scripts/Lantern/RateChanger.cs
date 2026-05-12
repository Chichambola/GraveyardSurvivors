using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RateChanger : MonoBehaviour
{
    [SerializeField] private LanternLight _light;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private float _shrinkRateIncrease = 0.1f;

    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += IncreaseRate;
        _enemyDetector.EnemyLeft += DecreaseRate;
    }

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= IncreaseRate;
        _enemyDetector.EnemyLeft -= DecreaseRate;
    }

    private void DecreaseRate(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception();
        
        float currentRate = _light.ShrinkRate;

        currentRate -= _shrinkRateIncrease;

        _light.SetRate(currentRate);
    }

    private void IncreaseRate(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception();
        
        float currentRate = _light.ShrinkRate;

        currentRate += _shrinkRateIncrease;
        
        _light.SetRate(currentRate);
    }
}
