using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : Detector
{
    private Dictionary<Collider, Enemy> _enemies;
    
    public event Action<Enemy> EnemyDetected; 
    public event Action<Enemy> EnemyLeft;

    private void Awake()
    {
        _enemies = new Dictionary<Collider, Enemy>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (_enemies.ContainsKey(other))
        {
            EnemyDetected?.Invoke(_enemies[other]);
        }
        else
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                _enemies.Add(other, enemy);
                
                EnemyDetected?.Invoke(enemy);
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (_enemies.ContainsKey(other))
        {
            EnemyLeft?.Invoke(_enemies[other]);
        }
        else
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                _enemies.Add(other, enemy);
                
                EnemyLeft?.Invoke(enemy);
            }
        }
    }
}
