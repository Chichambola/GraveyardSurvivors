using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RadiusResetter : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;
    
    private float _defaultValue = 0;
    private float _lastRadius;
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
    
    private void OnPlayerDetected(ILightCarrier carrier)
    {
        if (carrier == null)
            throw new Exception();

        carrier.ResetRadius();
    }

    private void OnPlayerLeft(ILightCarrier carrier)
    {
        if (carrier == null)
            throw new Exception();
        
        carrier.StartLight();
    }
}
