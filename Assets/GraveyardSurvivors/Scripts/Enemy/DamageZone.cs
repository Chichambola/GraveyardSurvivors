using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DamageZone : MonoBehaviour
{
    [SerializeField] private float _damageOnCollision = 3f;
    
    private BoxCollider _collider;
    private HashSet<Collider> _collidersToIgnore;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collidersToIgnore = new HashSet<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collidersToIgnore.Contains(other)) return;
        
        if (other.TryGetComponent(out IPlayer player) == false)
        {
            _collidersToIgnore.Add(other);
        }
        else
        {
            player.TakeDamage(_damageOnCollision);
        }
    }
}
