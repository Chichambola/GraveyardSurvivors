using System;
using System.Collections.Generic;
using Cinemachine;
using PrimeTween;
using Sherbert.Framework.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerHandler _playerHandler;
    
    [Header("Stats for upgrade")]
    [SerializeField] private SerializableDictionary<int, float> _minutesAndPercents;
    
    [Header("Services")]
    [SerializeField] private Darkness _darkness;
    [SerializeField] private LanternLight _lantern;
    [SerializeField] private EnemySpawnerHandler _enemySpawnerHandler;
    [SerializeField] private List<InteractableHandler> _interactables;
    
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _debugElapsedTime;
    

    private int _primeTweenCapacity = 3000;
    private float _elapsedTime;

    private void Awake()
    {
        PrimeTweenConfig.SetTweensCapacity(_primeTweenCapacity);
        _elapsedTime = _debugElapsedTime;
    }

    private void Update()
    {
        TimerController.UpdateTimers();

        UpdateTimer();
    }
    
    private void OnEnable()
    {
        _enemySpawnerHandler.EnemyWasKilled += OnEnemyDeath;
        
        if(_interactables == null) 
            throw new Exception("Interactables are null");
        
        Player player = _playerHandler.Spawn(_playerPrefab);
        
        _darkness.Init(player);
        _lantern.Init();
        _enemySpawnerHandler.SetPlayer(player);
        
        foreach (var interactables in _interactables)
        {
            if (interactables.TryGetComponent(out IInteractableHandler handler))
            {
                handler.Init(player);
            }
        }
    }

    private void OnDisable()
    {
        _enemySpawnerHandler.EnemyWasKilled -= OnEnemyDeath;
        _elapsedTime = 0;
        
        TimerController.Clear();
    }
    

    private void OnEnemyDeath(Enemy enemy)
    {
        _lantern.ProcessEnemyDeath(enemy);
    }
    
    private void UpdateTimer()
    {
        _elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        _timerText.text = $"{minutes:00} : {seconds:00}";

        if (_minutesAndPercents.Remove(minutes, out var percent))
        {
            _enemySpawnerHandler.Upgrade(percent);
        }
    }
}
