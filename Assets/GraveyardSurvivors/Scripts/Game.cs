using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor.Profiling;
using PrimeTween;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private int _targetFrameRate = 60;
    [SerializeField] private Player _player;
    [SerializeField] private Darkness _darkness;
    [SerializeField] private LanternLight _lantern;
    [SerializeField] private EnemySpawnerHandler _enemySpawnerHandler;
    [SerializeField] private List<InteractableHandler> _interactables;

    private int _primeTweenCapacity = 3000;

    private void Awake()
    {
        PrimeTweenConfig.SetTweensCapacity(_primeTweenCapacity);
    }

    private void Update()
    {
        TimerController.UpdateTimers();
    }

    private void OnEnable()
    {
        Application.targetFrameRate = _targetFrameRate;
        
        _enemySpawnerHandler.EnemyWasKilled += OnEnemyDeath;
        
        if(_interactables == null) 
            throw new Exception("Interactables are null");

        foreach (var interactables in _interactables)
        {
            if (interactables.TryGetComponent(out IInteractableHandler handler))
            {
                handler.Init(_player);
            }
        }
        
        _darkness.Init(_player);
        _lantern.Init();
    }

    private void OnDisable()
    {
        _enemySpawnerHandler.EnemyWasKilled -= OnEnemyDeath;
        
        TimerController.Clear();
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        _lantern.ProcessEnemyDeath(enemy);
    }
}
