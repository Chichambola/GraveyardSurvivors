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
        _enemyDetector.EnemyDetected += DecreaseDuration;
        _enemyDetector.EnemyLeft += IncreaseDuration;
    }

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= DecreaseDuration;
        _enemyDetector.EnemyLeft -= IncreaseDuration;
    }

    private void DecreaseDuration(Enemy enemy)
    {
        if (enemy == null) 
            throw new Exception();

        var decreaseValue = _light.CurrentDuration.GetClampedValueInverse(_shrinkRateIncrease);

        var duration = _light.CurrentDuration.SubtractPercentFromNumber(decreaseValue);

        _light.SetDuration(duration);
    }

    private void IncreaseDuration(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception();
        
        var increaseValue = _light.CurrentDuration.GetClampedValue(_shrinkRateIncrease, _light.InitialRadius);

        var duration = _light.CurrentDuration.AddPercentToNumber(increaseValue);

        _light.SetDuration(duration);
    }
}
