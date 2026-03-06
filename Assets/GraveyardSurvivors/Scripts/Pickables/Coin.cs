using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour, IThrowable, IPoolable<Coin>, IPickable
{
    [SerializeField] private Transform _aPoint;
    [SerializeField] private Transform _bPoint;
    [SerializeField] private Transform _cPoint;
    [SerializeField] private QuadraticCurvePoints _points;
    [SerializeField] private Thrower _thrower;
    
    public event Action<Coin> CanBeReleased;
    
    private Vector3 _initialForwardRotation;

    public Transform Transform => transform;
    
    public QuadraticCurvePoints Points => _points;

    private void Awake()
    {
        _points.SetPositions(_aPoint, _bPoint, _cPoint);
    }

    private void OnEnable()
    {
        _initialForwardRotation = transform.forward;
    }
    
    public void ResetCharacteristics()
    {
        transform.forward = _initialForwardRotation;
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public void StartMoving()
    {
        _thrower.StartThrowing(this, _points);
    }
}
