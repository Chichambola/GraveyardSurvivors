using System;
using System.Collections.Generic;
using AYellowpaper;
using Cinemachine;
using PrimeTween;
using Sherbert.Framework.Generic;
using TMPro;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private List<InterfaceReference<IHandler, MonoBehaviour>> _handlers;
    
    [Header("Item displaying")]
    [SerializeField] private ItemDisplayer _itemDisplayer;

    
    private IPlayer _player;
    private EnemySpawnerHandler _enemySpawnerHandler;
    private int _primeTweenCapacity = 3000;
    private static float s_normalTimeSpeed = 1;
    private static float s_pauseTime = 0.000001f;
    
    public static bool IsPaused { get; private set; }
    public static Game Instance { get; private set; }
    
    private void Awake()
    {
        PrimeTweenConfig.SetTweensCapacity(_primeTweenCapacity);
        PrimeTweenConfig.warnZeroDuration = false;
        
       if (Instance != null && Instance != this)
       {
           Destroy(gameObject);
           return;
       }
        
       Instance = this;
    }
    
    private void Update()
    {
        TimerController.UpdateTimers();
    }
    
    private void OnEnable()
    {
        _player = _playerHandler.Spawn(_playerPrefab);
        _player.PickedItem += OnItemPickedUp;
        _player.Died += OnPlayerDeath;

        foreach (var handler in _handlers)
        {
            handler.Value.Init(_player);

            if (handler.Value is EnemySpawnerHandler enemySpawnerHandler)
            {
                _enemySpawnerHandler = enemySpawnerHandler;
            }
        }

        if (_enemySpawnerHandler == null)
             throw new Exception("Enemy spawner handler should not be null");

        _enemySpawnerHandler.EnemyWasKilled += OnEnemyDeath;
        
        _lanternPointer.Init(_player, _lantern);
        _darkness.Init(_player);
    }

    private void OnDisable()
    {
        _enemySpawnerHandler.EnemyWasKilled -= OnEnemyDeath;
        _player.PickedItem -= OnItemPickedUp;
        
        TimerController.Clear();
        Tween.StopAll();
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

    private void OnPlayerDeath()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        SceneManager.LoadScene(currentSceneName);
    }
}
