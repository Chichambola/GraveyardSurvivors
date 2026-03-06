using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CollisionDetector : MonoBehaviour
{
    public event Action<float> EnemyDetected;
    
    private CapsuleCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            EnemyDetected?.Invoke(enemy.Damage);
        }
    }
}
