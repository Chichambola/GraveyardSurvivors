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
    
    private float _minRandomValue = -2f;
    private float _maxRandomValue = 5f;
    
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

    public void StartMoving()
    {
        _thrower.StartMoving(transform, _endPoint.position);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
