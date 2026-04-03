using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable<Projectile>
{
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private float _speedMultiplier;

    public event Action<Projectile> CanBeReleased;
    
    private IAttacker _currentTarget;
    private Coroutine _coroutine;

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
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(MovingRoutine());
    }

    private IEnumerator MovingRoutine()
    {
        while (enabled)
        {
            if (_currentTarget.Rigidbody.gameObject.activeSelf == false)
            {
                Release();
            }
            
            _mover.Move(_currentTarget.Rigidbody.transform, _speedMultiplier);

            Vector3 distance = _currentTarget.Rigidbody.transform.position - transform.position;

            Vector3 direction = new Vector3(distance.x, 0f, distance.z).normalized;
            
            _rotator.Rotate(direction);
            
            yield return null;
        }
    }
    
    public void ResetCharacteristics()
    {
        _currentTarget = null;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }
    
    public void SetTarget(IAttacker attacker)
    {
        _currentTarget = attacker ?? throw new Exception();
    }
    
    private void OnEnemyDetected(Enemy enemy)
    {
        if (enemy == (Enemy)_currentTarget)
        {
            Release();
        }
    }
}
