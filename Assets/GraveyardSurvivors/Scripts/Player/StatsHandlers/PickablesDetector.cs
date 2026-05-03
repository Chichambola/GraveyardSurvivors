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
    
    private SphereCollider _collider;
    private Dictionary<Collider, IPickable> _coins;
    private float _initialRadius;
    private float _currentRadiusMultiplier;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _coins = new Dictionary<Collider, IPickable>();
        _initialRadius = _collider.radius;
    }

    private void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
    }

    private void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (_coins.ContainsKey(other))
        {
            DeterminePickableType(_coins[other]);
        }
        else
        {
            if (other.TryGetComponent(out IPickable pickable))
            {
                if (pickable is Coin coin)
                {
                    _coins.Add(other, coin);
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
        
        pickable.Release();
    }

    private void OnStatsChanged(CharacterStats stats)
    {
        if (!Mathf.Approximately(stats.PickUpRadius, _currentRadiusMultiplier))
        {
            _currentRadiusMultiplier = stats.PickUpRadius;
        
            _collider.radius = _initialRadius.AddPercentToNumber(stats.PickUpRadius);
        }
    }
}

