using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttackArea : MonoBehaviour
{
    public event Action<IAttacker> EnemyDetected;
    
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    private void OnValidate()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IAttacker attacker))
        {
            EnemyDetected?.Invoke(attacker);
        }
    }

    public void SetVisibility(bool value)
    {
        _collider.enabled = value;
    }
}
