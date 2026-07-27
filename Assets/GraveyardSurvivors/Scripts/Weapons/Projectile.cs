using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable<Projectile>
{
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;

    public event Action<Projectile> CanBeReleased;
    public event Action<Projectile> HitEnemy;
    
    private IAttacker _currentTarget;
    private CancellationTokenSource _cts;

    public IAttacker CurrentTarget => _currentTarget;
    
    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += OnEnemyDetected;
    }

    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= OnEnemyDetected;
    }

    public void StartMoving()
    {
        if (_currentTarget == null)
            throw new Exception("Target is null");
        
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);
        
        MovingTask().Forget();   
    }
    
    public void ResetCharacteristics() => _currentTarget = null;

    public void Release() => CanBeReleased?.Invoke(this);
    
    public void SetTarget(IAttacker attacker) => _currentTarget = attacker ?? throw new Exception();
    
    private void OnEnemyDetected(Enemy enemy)
    {
        if (enemy != (Enemy)_currentTarget)
            return;
        
        HitEnemy?.Invoke(this);
            
        _cts?.Cancel();
    }
    
    private async UniTaskVoid MovingTask()
    {
        var target = _currentTarget as MonoBehaviour;

        if (target == null)
            throw new Exception();
        
        while (_currentTarget.IsAlive && !_cts.IsCancellationRequested)
        {
            _mover.MoveToPosition(target.transform.position);

            Vector3 distance = target.transform.position - transform.position;

            Vector3 direction = new Vector3(distance.x, 0f, distance.z).normalized;
            
            _rotator.Rotate(direction);

            await UniTask.WaitForFixedUpdate(_cts.Token);
        }
        
        Release();
    }
}
