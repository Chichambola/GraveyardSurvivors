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
    private Transform _currentTarget;

    public void StartMoving(Transform target)
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
            _mover.Move(_currentTarget, _speedMultiplier);

            Vector3 distance = _currentTarget.position - transform.position;

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

    public virtual void Release()
    {
        CanBeReleased?.Invoke(this);
    }
}
