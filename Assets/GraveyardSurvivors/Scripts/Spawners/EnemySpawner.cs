using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : Spawner<Enemy>, IEnemySpawner<Enemy>, IWeightedObject, IRestarter
{
    [SerializeField] private EnemyInfo _enemyInfo;
    [SerializeField] private EnemyStats _statsForUpgrade;
    [SerializeField] private bool _isAvailable;
    
    [Header("Loot spawners")]
    [SerializeField] private PickablesSpawner _coinSpawner;
    [SerializeField] private PickablesSpawner _crystalSpawner;
    
    [Header("Unit specific fields")]
    [SerializeField] private int _unitCost;
    [SerializeField] private int _weight;
    [SerializeField] private int _numberOfEnemiesSpawnAtOnce = 1;
    
    [Header("Unity workaround")]
    [SerializeField] private Vector3 _offsetAfterDeath = new (0, -90, 0);

    public event Action<Enemy> EnemyWasReleased;
    public event Action<Enemy> EnemyWasSpawned;
    
    private float _minRandomValue = -2f;
    private float _maxRandomValue = 4f;
    private Vector3 _spawnPoint;
    private IPlayer _player;
    private Enemy _enemyPrefab;
    private EnemyStats _baseStats;
    private List<Enemy> _spawnedUnits;

    public int Cost => _unitCost;
    public int Weight => _weight;
    public bool IsAvailable => _isAvailable;
    
    protected override void Awake()
    {
        base.Awake();

        _statsForUpgrade = new EnemyStats(_statsForUpgrade);

        _spawnedUnits = new List<Enemy>();
    }

    private void OnEnable()
    {
        RestartersHandler.Register(this);
        
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        _baseStats = _enemyInfo.GetStats();
        
        _enemyPrefab = Instantiate(ObjectPrefab, transform);

        _enemyPrefab.gameObject.SetActive(false);
        
        SetPrefab(_enemyPrefab);
    }

    private void OnDisable()
    {
        foreach (var enemy in _spawnedUnits)
        {
            enemy.ResetCharacteristics();
            
            if (enemy.isActiveAndEnabled)
            { 
                
                Release(enemy);
            }
        }
        
        _spawnedUnits.Clear();
        
        RestartersHandler.Deregister(this);
    }
    
    public void Spawn(Vector3 position)
    {
        for (int i = 0; i < _numberOfEnemiesSpawnAtOnce; i++)
        {
            _spawnPoint = position;
            
            GetObject();
        }
    }
    
    public void Restart()
    {
        Destroy(_enemyPrefab.gameObject);
        
        InitializeEnemy();
    }
    
    public void Upgrade()
    {
        _baseStats.SetStats(_statsForUpgrade);
        
        _enemyPrefab.Upgrade(_baseStats);

        foreach (var enemy in _spawnedUnits)
        {
            enemy.Upgrade(_baseStats);
        }
    }

    public void SetPlayer(IPlayer player) => _player = player;

    public void SetActive(bool isActive) => _isAvailable = isActive;

    protected override void ActionOnGet(Enemy enemy)
    {
        if (!_spawnedUnits.Contains(enemy))
        {
            _spawnedUnits.Add(enemy);
        }
        
        enemy.Init(_player, _baseStats);
        enemy.transform.parent = transform;
        enemy.transform.position = _spawnPoint.GetRandomOffsetPosition(_minRandomValue, _maxRandomValue);

        enemy.CanBeReleased += Release;
        enemy.NoHealthLeft += OnNoHealthLeft;
        
        ActiveObjects.Add(enemy);
        
        EnemyWasSpawned?.Invoke(enemy);
        
        base.ActionOnGet(enemy);
    }

    protected override void ActionOnRelease(Enemy enemy)
    {
        ActiveObjects.Remove(enemy);
        
        enemy.CanBeReleased -= Release;
        enemy.NoHealthLeft -= OnNoHealthLeft;
        
        base.ActionOnRelease(enemy);
        
        EnemyWasReleased?.Invoke(enemy);
        
        enemy.SetColliderCenter(_offsetAfterDeath, true);
    }

    protected override void ActionOnDestroy(Enemy enemy)
    {
        if (_spawnedUnits.Contains(enemy))
        {
            _spawnedUnits.Remove(enemy);
        }
        
        base.ActionOnDestroy(enemy);
    }

    private void OnNoHealthLeft(Enemy enemy)
    {
        enemy.SetColliderCenter(_offsetAfterDeath, false);
        _coinSpawner.Spawn(enemy.transform.position, enemy.CurrentStats.MoneyForKill);
        _crystalSpawner.Spawn(enemy.transform.position, enemy.CurrentStats.XpForKill);
    }
}
