using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] private QuadraticCurve _curve;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _speed;

    public event Action<IThrowable> FinishedMoving;
    
    private Coroutine _coroutine; 
    private readonly float _sampleTime = 0f;

    public void StartThrowing(IThrowable throwable, QuadraticCurvePoints curvePoints)
    {
        _curve.SetPointsPosition(curvePoints);
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);
        
        StartCoroutine(ThrowCoroutine(throwable));
    }
    
    private IEnumerator ThrowCoroutine(IThrowable throwable)
    {
        float initialSampleTime = _sampleTime;
        int finishValue = 1;

        while (initialSampleTime < finishValue)
        {        
            initialSampleTime += Time.deltaTime * _speed;
            
            throwable.Rigidbody.transform.position = _curve.Evaluate(initialSampleTime);
            throwable.Rigidbody.transform.forward = _curve.Evaluate(initialSampleTime + _rotationSpeed) - throwable.Rigidbody.transform.position;
            
            yield return null;
        }

        FinishedMoving?.Invoke(throwable);
        
        yield return null;
    }
}
