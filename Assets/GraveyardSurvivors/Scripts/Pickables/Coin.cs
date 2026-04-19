using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour, IThrowable, IPoolable<Coin>, IPickable
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Thrower _thrower;

    [SerializeField] private int _value = 1;
    [SerializeField] private float _timeBeforeRelease = 2f;
    
    private Coroutine _coroutine;
    private Color _originalColor;
    
    public event Action<Coin> CanBeReleased;
    
    private Vector3 _initialForwardRotation;
    
    public int Value => _value;
    
    private void OnEnable()
    {
        _initialForwardRotation = transform.forward;

        _thrower.FinishedMoving += OnFinishedMoving;
    }

    private void OnDisable()
    {
        _thrower.FinishedMoving -= OnFinishedMoving;
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
        _thrower.StartMoving(transform, _endPoint.position);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void OnFinishedMoving()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangingOpacity());
    }

    private IEnumerator ChangingOpacity()
    {
        var wait = new WaitForSeconds(_timeBeforeRelease);
        
        while (enabled)
        {
            yield return wait;
            
            Release();
        }
    }
}
