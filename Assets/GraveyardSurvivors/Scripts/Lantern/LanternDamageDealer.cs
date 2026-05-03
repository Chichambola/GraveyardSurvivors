using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternDamageDealer : MonoBehaviour
{
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private float _damage = 3f;
    [SerializeField] private float _rate = 1f;

    private int _time = 3600;
    private IntervalTimer _timer;
    
    public event Action EnemyDetected;
    public event Action EnemyLeft;
    public event Action<Enemy> EnemyDied;
    
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

        _enemyDetector.EnemyDetected += OnEnemyDetected;
        _enemyDetector.EnemyLeft += OnEnemyLeft;
    }

    private void OnDisable()
    {
        _timer.Stop();
        _enemyDetector.EnemyDetected -= OnEnemyDetected;
        _enemyDetector.EnemyLeft -= OnEnemyLeft;
    }

    private void OnEnemyLeft(Enemy enemy)
    {
        if (_enemiesInRange.Contains(enemy))
        {
            _enemiesInRange.Remove(enemy);
            
            EnemyLeft?.Invoke();
        }
    }

    private void OnEnemyDetected(Enemy enemy)
    {
        if (_enemiesInRange.Contains(enemy) == false)
        {
            _enemiesInRange.Add(enemy);
            
            EnemyDetected?.Invoke();
        }
    }

    private void DamageEnemies()
    {
        if (_enemiesInRange.Count > 0)
        {
            for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
            {
                _enemiesInRange[i].TakeDamage(_damage);

                IsEnemyDead(_enemiesInRange[i]);
            }
        }
    }

    private void IsEnemyDead(Enemy enemy)
    {
        if (enemy.CurrentStats.Health <= 0)
        {
            OnEnemyLeft(enemy);
                        
            EnemyDied?.Invoke(enemy);
        }
    }
}