using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private EnemyDetector _enemyDetector;
    
    public event Action<float> EnemyDetected;
    
    private CapsuleCollider _collider;

    
    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        _enemyDetector.EnemyDetected += OnEnemyDetected;
    }
    
    private void OnDisable()
    {
        _enemyDetector.EnemyDetected -= OnEnemyDetected;
    }

    private void OnEnemyDetected(Enemy enemy)
    {
        EnemyDetected?.Invoke(enemy.DamageOnCollision);
    }
}
