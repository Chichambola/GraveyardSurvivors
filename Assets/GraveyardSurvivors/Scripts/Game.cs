using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor.Profiling;
using UnityEngine.Rendering;
using PrimeTween;
using Sherbert.Framework.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [Header("Player and experience")]
    [SerializeField] private Player _player;
    [SerializeField] private ExperienceHandler _experienceHandler;

    [Header("Stats for upgrade")]
    [SerializeField] private float _healthIncrease;
    [SerializeField] private float _healthRegnerationIncrease;
    [SerializeField] private float _baseDamageIncrease;
    
    [Header("Services")]
    [SerializeField] private Darkness _darkness;
    [SerializeField] private LanternLight _lantern;
    [SerializeField] private EnemySpawnerHandler _enemySpawnerHandler;
    [SerializeField] private List<InteractableHandler> _interactables;
    [SerializeField] private TextMeshProUGUI _timerText;

    private int _primeTweenCapacity = 3000;
    private float _elapsedTime;

    private void Awake()
    {
        PrimeTweenConfig.SetTweensCapacity(_primeTweenCapacity);
    }

    private void Update()
    {
        TimerController.UpdateTimers();

        _elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        _timerText.text = $"{minutes:00} : {seconds:00}";
    }

    private void OnEnable()
    {
        _enemySpawnerHandler.EnemyWasKilled += OnEnemyDeath;
        _experienceHandler.PlayerReachedThreshold += OnPlayerReachedThreshold;
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
        _experienceHandler.PlayerReachedThreshold -= OnPlayerReachedThreshold;
        _player.GainedXp -= OnPlayerGainedXp;
        
        TimerController.Clear();
    }

    private void OnPlayerReachedThreshold()
    {
        _player.CurrentStats.Health = _player.CurrentStats.Health.AddPercentToNumber(_healthIncrease);
        _player.CurrentStats.MaxHealth = _player.CurrentStats.MaxHealth.AddPercentToNumber(_healthIncrease);
        _player.CurrentStats.HealthRegeneration = _player.CurrentStats.HealthRegeneration.AddPercentToNumber(_healthRegnerationIncrease);
        _player.IncreaseDamage(_baseDamageIncrease);
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
