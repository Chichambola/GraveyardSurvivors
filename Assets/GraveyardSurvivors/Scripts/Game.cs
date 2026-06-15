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
    
    [Header("Stats for enemy spawners")]
    [SerializeField] private SerializableDictionary<int, float> _minutesAndPercents;
    
    [Header("Services")]
    [SerializeField] private Darkness _darkness;
    [SerializeField] private LanternLight _lantern;
    [SerializeField] private LightPointer _lanternPointer;
    [SerializeField] private EnemySpawnerHandler _enemySpawnerHandler;
    [SerializeField] private InteractablesHandler _interactablesHandler;
    [SerializeField] private ItemDisplayer _itemDisplayer;
    
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _debugElapsedTime;
    

    private int _primeTweenCapacity = 3000;
    private float _elapsedTime;
    private IPlayer _player;

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
    
    private void OnItemPickedUp(Item item)
    {
        _itemDisplayer.Enqueue(item);
    }
}
