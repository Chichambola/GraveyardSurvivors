using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable<Projectile>
{
    [SerializeField] private Mover _mover;
    [SerializeField] private float _speedMultiplier;

    public event Action<Projectile> CanBeReleased;
    
    private Coroutine _coroutine;
    private Transform _currentTarget;

    public void StartMoving(Transform target)
    {
        if (target == null)
            throw new Exception();

        _currentTarget = target;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(MovingRoutine());
    }

    private IEnumerator MovingRoutine()
    {
        while (enabled)
        {
            _mover.MoveTowardsTarget(_currentTarget, _speedMultiplier);
            
            yield return null;
        }
    }
    
    public void ResetCharacteristics()
    {
        _currentTarget = null;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public virtual void Release()
    {
        CanBeReleased?.Invoke(this);
    }
}
