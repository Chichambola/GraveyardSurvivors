using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Target : MonoBehaviour, ITarget
{
    public event Action WasReached;

    private IFollower _follower;
    private Rigidbody _rigidbody;
    private BoxCollider _collider;

    public bool HasFollower => _follower != null;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    private void OnValidate()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnDisable()
    {
        _follower = null;
    }

    public void SetFollower(IFollower follower)
    {
        _follower = follower;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IFollower follower))
            return;

        if (follower != _follower)
            return;
        
        WasReached?.Invoke();

        _follower = null;
    }
}
