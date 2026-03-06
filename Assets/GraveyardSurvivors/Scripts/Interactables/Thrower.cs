using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Thrower : MonoBehaviour
{
    [SerializeField] private QuadraticCurve _curve;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _speed;

    public event Action FinishedMoving;
    
    private Coroutine _coroutine; 

    public void StartThrowing(IThrowable throwable, QuadraticCurvePoints curvePoints)
    {
        _curve.SetPointsPosition(curvePoints);
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);
        
        StartCoroutine(ThrowCoroutine(throwable));
    }
    
    private IEnumerator ThrowCoroutine(IThrowable throwable)
    {
        float initialSampleTime = 0f;
        int finishValue = 1;

        while (initialSampleTime <= finishValue)
        {        
            initialSampleTime += Time.deltaTime * _speed;
            
            throwable.Transform.position = _curve.Evaluate(initialSampleTime);
            throwable.Transform.forward = _curve.Evaluate(initialSampleTime + _rotationSpeed) - throwable.Transform.position;
            
            yield return null;
        }
        
        FinishedMoving?.Invoke();
        
        yield return null;
    }
}
