using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class ItemPlaceholder : MonoBehaviour, IThrowable, IPoolable<ItemPlaceholder>
{
    private Rigidbody _rigidbody;

    public event Action<ItemPlaceholder> CanBeReleased;
    public Rigidbody Rigidbody => _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void ResetCharacteristics() { }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }
}
