using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Thrower : MonoBehaviour
{
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private int _numberofJumps = 1;

    public event Action FinishedMoving;

    private Sequence _throwerTween;
    private float _minRandomValue = -2f;
    private float _maxRandomValue = 5f;

    public void StopMoving()
    {
        FinishedMoving?.Invoke();
        
        _throwerTween.Stop();
    }
    
    public void StartMoving(Transform throwable, Vector3 endPoint, bool isRandomPosition = false)
    {
        endPoint = GetRandomPosition(endPoint, isRandomPosition);

        _throwerTween = PrimeTweenExtension.Jump(throwable, endPoint, _duration, _jumpPower).OnComplete(StopMoving);
    }

    private Vector3 GetRandomPosition(Vector3 endPoint, bool isRandomPosition)
    {
        if (isRandomPosition)
        {
            endPoint.x += Random.Range(_minRandomValue, _maxRandomValue);
            endPoint.z += Random.Range(_minRandomValue, _maxRandomValue);
        }

        return endPoint;
    }
}