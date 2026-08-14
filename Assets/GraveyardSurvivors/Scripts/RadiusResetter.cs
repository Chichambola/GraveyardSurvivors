using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusResetter : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private float _speed = 3f;

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
        _player = player ?? throw new Exception(nameof(player));
        
        _player.ResetRadius(_speed);
    }

    private void OnPlayerLeft()
    {
        _player.StartLight();

        _player = null;
    }
}
