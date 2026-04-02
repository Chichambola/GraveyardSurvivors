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
    
    protected IAttacker CurrentTarget;
    protected float Damage;
    private Coroutine _coroutine;

    public void StartMoving()
    {
        if (CurrentTarget == null)
            throw new Exception("Target is null");
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(MovingRoutine());
    }

    private IEnumerator MovingRoutine()
    {
        while (enabled)
        {
            if (CurrentTarget.Rigidbody.gameObject.activeSelf == false)
            {
                Release();
            }
            
            _mover.Move(CurrentTarget.Rigidbody.transform, _speedMultiplier);

            Vector3 distance = CurrentTarget.Rigidbody.transform.position - transform.position;

            Vector3 direction = new Vector3(distance.x, 0f, distance.z).normalized;
            
            _rotator.Rotate(direction);
            
            yield return null;
        }
    }
    
    public void ResetCharacteristics()
    {
        CurrentTarget = null;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }
    
    public void SetTarget(IAttacker attacker)
    {
        CurrentTarget = attacker ?? throw new Exception();
    }
}
