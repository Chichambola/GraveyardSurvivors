using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RateChanger : MonoBehaviour
{
    [SerializeField] private LanternLight _light;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private float _shrinkRateIncrease = 5;

    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += DecreaseSpeed;
        _enemyDetector.EnemyLeft += IncreaseSpeed;
    }

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= DecreaseSpeed;
        _enemyDetector.EnemyLeft -= IncreaseSpeed;
    }

    private void IncreaseSpeed(Enemy enemy)
    {
        if (enemy == null) 
            throw new Exception();
        
        _light.IncreaseSpeed(_shrinkRateIncrease);
    }

    private void DecreaseSpeed(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception();

        _light.DecreaseSpeed(_shrinkRateIncrease);
    }
}
