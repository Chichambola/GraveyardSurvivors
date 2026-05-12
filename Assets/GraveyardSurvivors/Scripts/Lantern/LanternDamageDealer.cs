using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanternDamageDealer : MonoBehaviour
{
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private float _damage = 3f;
    [SerializeField] private float _rate = 1f;

    private int _time = 3600;
    private IntervalTimer _timer;
    
    private List<Enemy> _enemiesInRange;

    private void Awake()
    {
        _enemiesInRange = new List<Enemy>();
    }

    private void OnEnable()
    {
        _timer = new IntervalTimer(_time, _rate);
        _timer.IntervalReached += DamageEnemies;
        _timer.Start();

        _enemyDetector.EnemyLeft += OnEnemyLeft;
        _enemyDetector.EnemyDetected += _enemiesInRange.Add;
    }

    private void OnDisable()
    {
        _timer?.Stop();

        _enemyDetector.EnemyLeft -= OnEnemyLeft;
        _enemyDetector.EnemyDetected -= _enemiesInRange.Add;
    }

    private void OnEnemyLeft(Enemy enemy)
    {
        if (_enemiesInRange.Contains(enemy))
        {
            _enemiesInRange.Remove(enemy);   
        }
    }

    private void DamageEnemies()
    {
        if (_enemiesInRange.Count > 0)
        {
            for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
            {
                _enemiesInRange[i].TakeDamage(_damage);
            }
        }
    }
}