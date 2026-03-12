using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternDamageDealer : MonoBehaviour
{
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private float _damage = 3f;
    [SerializeField] private float _rate = 1f;

    public event Action<float> DamageDealt;
    
    private List<Enemy> _enemiesInRange;
    private Coroutine _coroutine;

    private void Awake()
    {
        _enemiesInRange = new List<Enemy>();
    }

    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += OnEnemyDetected;
        _enemyDetector.EnemyLeft += OnEnemyLeft;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(DamageRoutine());
    }
    

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= OnEnemyDetected;
        _enemyDetector.EnemyLeft -= OnEnemyLeft;
    }
    
    private void OnEnemyLeft(Enemy enemy)
    {
        enemy.CanBeReleased -= OnEnemyLeft;
        
        if (_enemiesInRange.Contains(enemy))
        {
            _enemiesInRange.Remove(enemy);   
        }
    }

    private void OnEnemyDetected(Enemy enemy)
    {
        enemy.CanBeReleased += OnEnemyLeft;
        
        _enemiesInRange.Add(enemy); 
    }

    private IEnumerator DamageRoutine()
    {
        var wait = new WaitForSecondsRealtime(_rate);
        
        while (enabled)
        {
            float damagePercent = 0f;
            
            if (_enemiesInRange.Count > 0)
            {
                foreach (var enemy in _enemiesInRange)
                {
                    enemy.TakeDamage(_damage);

                    damagePercent += enemy.CurrentStats.LanternEnergy;
                }
            }
            
            DamageDealt?.Invoke(damagePercent);
            
            yield return wait;
        }
    }
}
