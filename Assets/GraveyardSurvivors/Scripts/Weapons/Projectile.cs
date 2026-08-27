using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable<Projectile>
{
    [SerializeField] private Follower _follower;
    [SerializeField] private EnemyDetector _enemyDetector;
    
    public event Action<Projectile> CanBeReleased;
    public event Action<Projectile> HitEnemy;
    
    private ITarget _target;

    public ITarget Target => _target;
    
    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += OnEnemyDetected;
    }

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= OnEnemyDetected;
    }
    
    public void ResetCharacteristics() => _target = null;

    public void Release() => CanBeReleased?.Invoke(this);
    
    public void StartMoving()
    {
        _follower.StartMoving();
    }
    
    public void SetTarget(ITarget target)
    {
        _target = target;
        
        _follower.SetTarget(_target);
    }

    private void OnEnemyDetected(Enemy enemy)
    {
        if (enemy != (Enemy)_target)
            return;
        
        HitEnemy?.Invoke(this);
            
        _follower.StopMoving();
    }
}
