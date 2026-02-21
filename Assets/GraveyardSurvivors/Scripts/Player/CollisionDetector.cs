using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CollisionDetector : Detector
{
    public event Action<IBuff> ItemDetected;
    
    private CapsuleCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            if (item is IBuff buff)
            {
                item.Release();
                
                ItemDetected?.Invoke(buff);   
            }
        }
    }

    protected override void OnTriggerExit(Collider other) { }
}
