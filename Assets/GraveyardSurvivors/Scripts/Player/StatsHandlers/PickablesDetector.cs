using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class PickablesDetector : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ItemDisplayer _itemDisplayer;
    
    public event Action<IBuff> BuffDetected;
    public event Action<float> CoinDetected;
    public event Action<float> CrystalDetected; 
    
    private SphereCollider _collider;
    private float _initialRadius;
    private float _currentRadiusMultiplier;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
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
        if (other.TryGetComponent(out IPickable pickable))
        {
            DeterminePickableType(pickable);
        }
    }

    private void DeterminePickableType(IPickable pickable)
    {
        if (pickable is IBuff buff)
        {
            BuffDetected?.Invoke(buff);

            if (buff is Item item)
            {
                _itemDisplayer.Enqueue(item);
            }
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

