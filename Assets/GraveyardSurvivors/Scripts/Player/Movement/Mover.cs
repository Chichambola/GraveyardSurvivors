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
    private Coroutine _coroutine;
    
    public bool IsRunning { get; private set; }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveTowardsTarget(Transform target, float speedMultiplier)
    {
        float currentSpeed = UserUtils.AddPercentToNumber(_speed, speedMultiplier);
        
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.fixedDeltaTime);
    }
    
    public void Move(Vector3 direction, float speedMultiplier)
    {
        float currentSpeed = UserUtils.AddPercentToNumber(_speed, speedMultiplier);
        
        Vector3 nextPosition = _rigidbody.position + direction * (Time.fixedDeltaTime * currentSpeed);
        
        _rigidbody.MovePosition(nextPosition);
    }
}
