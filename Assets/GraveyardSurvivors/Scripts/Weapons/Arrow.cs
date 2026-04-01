using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private EnemyDetector _enemyDetector;
    
    private float _damage;

    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += OnEnemyDetected;
    }

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= OnEnemyDetected;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    
    public override void Release()
    {
        _damage = 0;
        
        base.Release();
    }
    
    private void OnEnemyDetected(Enemy enemy)
    {
        if (_damage > 0)
        {
            enemy.TakeDamage(_damage);
            
            Release();
        }
        else if (_damage <= 0)
        {
            throw new Exception();   
        }
    }
}
