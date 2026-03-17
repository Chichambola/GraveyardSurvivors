using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternDamageDealer : MonoBehaviour
{
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private float _damage = 3f;
    [SerializeField] private float _rate = 1f;

    public event Action EnemyDetected;
    public event Action EnemyLeft;
    public event Action<Enemy> EnemyDied;
    
    private List<Enemy> _enemiesInRange;
    private Coroutine _coroutine;

    private void Awake()
    {
        _enemiesInRange = new List<Enemy>();
    }

    private void OnEnable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(DamageRoutine());

        _enemyDetector.EnemyDetected += OnEnemyDetected;
        _enemyDetector.EnemyLeft += OnEnemyLeft;
    }

    private void OnDisable()
    {
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
        EnemyDetected?.Invoke();

        _enemiesInRange.Add(enemy);  
    }
    
    private IEnumerator DamageRoutine()
    {
        var wait = new WaitForSecondsRealtime(_rate);
        
        while (enabled)
        {
            if (_enemiesInRange.Count > 0)
            {
                for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
                {
                    _enemiesInRange[i].TakeDamage(_damage);

                    IsEnemyDead(_enemiesInRange[i]);
                }
            }
            
            yield return wait;
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