using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PickablesDetector : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    public event Action<IBuff> BuffDetected;
    public event Action<float> CoinDetected;
    public event Action<float> CrystalDetected; 
    
    private SphereCollider _collider;
    private Dictionary<Collider, IPickable> _pickables;
    private float _initialRadius;
    private float _currentRadiusMultiplier;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _pickables = new Dictionary<Collider, IPickable>();
        _initialRadius = _collider.radius;
    }

    private void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
    }

    private void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
        _collider.radius = _initialRadius;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (_pickables.ContainsKey(other))
        {
            DeterminePickableType(_pickables[other]);
        }
        else
        {
            if (other.TryGetComponent(out IPickable pickable))
            {
                if (pickable is not Item)
                {
                    _pickables.Add(other, pickable);
                }
                
                DeterminePickableType(pickable);
            }
        }
    }

    private void DeterminePickableType(IPickable pickable)
    {
        if (pickable is IBuff buff)
        {
            BuffDetected?.Invoke(buff);
        }

        if (pickable is Coin coin)
        {
            CoinDetected?.Invoke(coin.Value);
        }

        if (pickable is Crystal crystal)
        {
            CrystalDetected?.Invoke(crystal.Value);
        }
        
        pickable.Release();
    }

    private void OnStatsChanged(CharacterStats stats)
    {
        if (!Mathf.Approximately(stats.PickUpRadius, _currentRadiusMultiplier))
        {
            _currentRadiusMultiplier = stats.PickUpRadius;
        
            _collider.radius = _collider.radius.AddPercentToNumber(stats.PickUpRadius);
        }
    }
}

