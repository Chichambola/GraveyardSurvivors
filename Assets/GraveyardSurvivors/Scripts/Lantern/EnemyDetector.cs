using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetector : Detector
{
    public event Action<Enemy> EnemyDetected; 
    public event Action<Enemy> EnemyLeft; 
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            EnemyDetected?.Invoke(enemy);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            EnemyLeft?.Invoke(enemy);
        }
    }
}
