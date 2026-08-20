using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusResetter : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private float _speed = 3f;

    private ILightCarrier _carrier;
    
    private void OnEnable()
    {
        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerLeft += OnPlayerLeft;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerDetected -= OnPlayerDetected;
        _playerDetector.PlayerLeft -= OnPlayerLeft;
    }
    
    private void OnPlayerDetected(IPlayer player)
    {
        if (player is not ILightCarrier carrier)
            return;
        
        _carrier = carrier;
        
        _carrier.ResetRadius(_speed);
    }

    private void OnPlayerLeft()
    {
        _carrier.StartChangingRadius();

        _carrier = null;
    }
}
