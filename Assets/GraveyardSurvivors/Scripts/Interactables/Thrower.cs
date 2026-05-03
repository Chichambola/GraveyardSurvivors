using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class Thrower : MonoBehaviour
{
    [SerializeField] private Ease _ease = Ease.Linear;
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private int _numberofJumps = 1;

    public event Action FinishedMoving;

    private Sequence _throwerTween;

    public void StartMoving(Transform throwable, Vector3 endPoint)
    {
        _throwerTween = throwable.DOJump(endPoint, _jumpPower,_numberofJumps, _duration).SetEase(_ease).OnComplete(()=>FinishedMoving?.Invoke());
    }

    public void StopMoving()
    {
        _throwerTween?.Kill();
    }
}