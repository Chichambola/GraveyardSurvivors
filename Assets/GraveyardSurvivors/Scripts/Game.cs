using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor.Profiling;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Darkness _darkness;
    [SerializeField] private Lantern _lantern;
    [SerializeField] private EnemySpawnerHandler _enemySpawnerHandler;
    [SerializeField] private List<InteractableHandler> _interactables;

    private void Update()
    {
        TimerController.UpdateTimers();
    }

    private void OnEnable()
    {
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
