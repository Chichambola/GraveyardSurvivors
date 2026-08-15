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

    private List<Enemy> _enemies;
    
    private void Awake()
    {
        _enemies = new List<Enemy>();
    }

    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += IncreaseSpeed;
        _enemyDetector.EnemyLeft += DecreaseSpeed;
    }

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= IncreaseSpeed;
        _enemyDetector.EnemyLeft -= DecreaseSpeed;
    }

    private void IncreaseSpeed(Enemy enemy)
    {
        if (enemy == null) 
            throw new Exception();
        
        _enemies.Add(enemy);
        
        _light.ChangeSpeed(_shrinkRateIncrease, _enemies.Count);
    }

    private void DecreaseSpeed(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception();

        _enemies.Remove(enemy);
        
        _light.ChangeSpeed(_shrinkRateIncrease, _enemies.Count);
    }
}
