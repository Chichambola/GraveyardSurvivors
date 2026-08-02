using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusResetter : MonoBehaviour
{
    [SerializeField] private PlayerDetector _playerDetector;

    private IPlayer _player;
    
    private void OnEnable()
    {
        _playerDetector.PlayerDetected += OnPlayerDetected;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerDetected -= OnPlayerDetected;
    }
    
    private void OnPlayerDetected(IPlayer player)
    {
        _player = player ?? throw new Exception(nameof(player));
        
        _player.ResetRadius();
    }
}
