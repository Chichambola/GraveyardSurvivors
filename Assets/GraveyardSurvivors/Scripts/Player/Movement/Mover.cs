using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;

    public float Speed => _speed;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Transform target, float speedMultiplier)
    {
        float currentSpeed = UserUtils.AddPercentToNumber(_speed, speedMultiplier);
        
        Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }
    
    public void Move(Vector3 direction, float speedMultiplier)
    {
        float currentSpeed = UserUtils.AddPercentToNumber(_speed, speedMultiplier);
        
        Vector3 nextPosition = _rigidbody.position + direction * (Time.deltaTime * currentSpeed);
        
        _rigidbody.MovePosition(nextPosition);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
