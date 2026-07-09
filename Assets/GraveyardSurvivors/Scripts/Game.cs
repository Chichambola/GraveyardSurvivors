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
    [Header("Player scripts")]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private PlayerHandler _playerHandler;
    
    [Header("Darkness and lantern stuff")]
    [SerializeField] private Darkness _darkness;
    [SerializeField] private LanternLight _lantern;
    [SerializeField] private LightPointer _lanternPointer;
    
    [Header("Handlers")]
    [SerializeField] private EnemySpawnerHandler _enemySpawnerHandler;
    [SerializeField] private InteractablesHandler _interactablesHandler;
    
    [Header("Item displaying")]
    [SerializeField] private ItemDisplayer _itemDisplayer;
    
    private int _primeTweenCapacity = 3000;
    private static float s_normalTimeSpeed = 1;
    private static float s_pauseTime = 0.00001f;
    private IPlayer _player;
    
    public static bool IsPaused { get; private set; }
    
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
        
        _player = _playerHandler.Spawn(_playerPrefab);

        _player.PickedItem += OnItemPickedUp;
        
        _enemySpawnerHandler.SetPlayer(_player);
        _interactablesHandler.Init(_player);
        _lanternPointer.Init(_player, _lantern);
        _darkness.Init(_player);
        _lantern.Init();
    }

    private void OnDisable()
    {
        _enemySpawnerHandler.EnemyWasKilled -= OnEnemyDeath;
        _player.PickedItem -= OnItemPickedUp;
        
        TimerController.Clear();
    }

    public static void Pause()
    {
        IsPaused = true;
        Time.timeScale = s_pauseTime;
    }

    public static void Resume()
    {
        IsPaused = false;
        Time.timeScale = s_normalTimeSpeed;
    }
    
    private void OnItemPickedUp(Item item)
    {
        _itemDisplayer.Process(item);
    }
    
    private void OnEnemyDeath(Enemy enemy)
    {
        _lantern.ProcessEnemyDeath(enemy);
    }
}
