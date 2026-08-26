using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class ItemPlaceholder : MonoBehaviour, IThrowable, IPoolable<ItemPlaceholder>
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Thrower _thrower;
    
    public event Action<ItemPlaceholder> CanBeReleased;
    
    private void OnEnable()
    {
        _thrower.FinishedMoving += Release;
    }

    private void OnDisable()
    {
        _thrower.FinishedMoving -= Release;
    }

    public void ResetCharacteristics()
    {
        
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public void StartThrowing()
    {
        _thrower.StartMoving(transform, _endPoint.position);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
