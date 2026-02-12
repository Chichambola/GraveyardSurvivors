using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] private QuadraticCurve _curve;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _speed;
    
    private Coroutine _coroutine; 
    private float _sampleTime = 0f;

    public void StartThrowing(Item item, QuadraticCurvePoints curvePoints)
    {
        _curve.SetPointsPosition(curvePoints);
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);
        
        StartCoroutine(ThrowCoroutine(item));
    }
    
    private IEnumerator ThrowCoroutine(Item item)
    {
        float initialSampleTime = _sampleTime;
        float finishValue = 1f;

        while (!Mathf.Approximately(initialSampleTime, finishValue))
        {        
            initialSampleTime += Time.fixedDeltaTime * _speed;
            
            item.transform.position = _curve.Evaluate(initialSampleTime);
            item.transform.forward = _curve.Evaluate(initialSampleTime + _rotationSpeed) - item.transform.position;
            
            yield return null;
        }
    }
}
