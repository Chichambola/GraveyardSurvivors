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
    [Header("Player and experience")]
    [SerializeField] private Player _player;
    [SerializeField] private ExperienceHandler _experienceHandler;
    [Header("Services")]
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
        _enemySpawnerHandler.EnemyWasKilled += OnEnemyDeath;
        _player.GainedXp += OnPlayerGainedXp;
        
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
        _player.GainedXp -= OnPlayerGainedXp;
        
        TimerController.Clear();
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        _lantern.ProcessEnemyDeath(enemy);
    }
    
    private void OnPlayerGainedXp(float value)
    {
        float tempXp = _player.CurrentStats.XpMultiplier * value;

        tempXp = tempXp.RoundToTenths();
        
        _experienceHandler.GainExperience(tempXp);
    }
}
