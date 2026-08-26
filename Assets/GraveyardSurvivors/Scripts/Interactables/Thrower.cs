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

    private Sequence _sequence;
    
    public void StartMoving(Transform throwable, Vector3 endPoint)
    {
        _sequence = Sequence.Create()
            .Group(PrimeTweenExtension.Jump(throwable, endPoint, _duration, _jumpPower, _numberofJumps).OnComplete(StopMoving));
    }
    
    private void StopMoving()
    {
        FinishedMoving?.Invoke();
        
        _sequence.Stop();
    }
}