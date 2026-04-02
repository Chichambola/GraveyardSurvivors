using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exception = System.Exception;

public class Arrow : Projectile
{
    [SerializeField] private EnemyDetector _enemyDetector;

    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += OnEnemyDetected;
    }

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= OnEnemyDetected;
    }
    
    private void OnEnemyDetected(Enemy enemy)
    {
        if (enemy == (Enemy)CurrentTarget)
        {
            if (Damage < 0)
            {
                throw new Exception("Damage can not be less than 0");
            }
            
            enemy.TakeDamage(Damage);
        }
        
        Release();
    }
}
