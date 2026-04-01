using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable<Projectile>
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private float _speedMultiplier;

    public event Action<Projectile> CanBeReleased;
    
    private Coroutine _coroutine;
    private IAttacker _currentTarget;

    public void StartMoving(IAttacker target)
    {
        _currentTarget = target ?? throw new Exception();
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(MovingRoutine());
    }

    private IEnumerator MovingRoutine()
    {
        while (enabled)
        {
            _mover.Move(_currentTarget.Rigidbody.position, _speedMultiplier);
            
            _rotator.Rotate(_currentTarget.Rigidbody.po);
            
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
