using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RadiusResetter : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;
    
    private float _lastRadius;
    private IPlayer _player;

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
        _player = player ?? throw new Exception();

        _player.ResetRadius();
    }

    private void OnPlayerLeft()
    {
        _player.StartLight();

        _player = null;
    }
}
