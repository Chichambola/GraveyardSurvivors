using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    public const int MaxPercent = 100;
    
    [SerializeField] private float _speed;
    
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction, float speedMultiplier)
    {
        float currentSpeed = _speed * (speedMultiplier / MaxPercent);
        
        Vector3 nextPosition = _rigidbody.position + direction * (Time.fixedDeltaTime * currentSpeed);
        
        _rigidbody.MovePosition(nextPosition);
    }
}
