using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class EnemySpawner : Spawner<Enemy>, IEnemySpawner<Enemy>, IWeightedObject
{
    [SerializeField] private Player _player;
    [Header("Loot spawners")]
    [SerializeField] private PickablesSpawner _coinSpawner;
    [SerializeField] private PickablesSpawner _crystalSpawner;
    [Header("Unit specific fields")]
    [SerializeField] private int _unitCost;
    [SerializeField] private int _weight;
    [SerializeField] private int _numberOfEnemiesSpawnAtOnce = 1;
    [Header("Unity workaround")]
    [SerializeField] private Vector3 _offsetAfterDeath = new (0, -90, 0);

    private Vector3 _spawnPoint;

    public event Action<Enemy> EnemyWasReleased;
    public event Action<Enemy> EnemyWasSpawned;
    
    public int Cost => _unitCost;
    public int Weight => _weight;

    public void Spawn(Vector3 position)
    {
        for (int i = 0; i < _numberOfEnemiesSpawnAtOnce; i++)
        {
            _spawnPoint = position;
            
            GetObject();
        }
    }
    
    protected override void ActionOnGet(Enemy enemy)
    {
        enemy.Init(_player);
        enemy.transform.position = _spawnPoint;
        enemy.transform.parent = transform;
        
        enemy.CanBeReleased += Release;
        enemy.NoHealthLeft += OnNoHealthLeft;
        
        ActiveObjects.Add(enemy);
        
        EnemyWasSpawned?.Invoke(enemy);
        
        base.ActionOnGet(enemy);
    }

    protected override void ActionOnRelease(Enemy enemy)
    {
        ActiveObjects.Remove(enemy);
        
        enemy.ResetCharacteristics();
        
        enemy.CanBeReleased -= Release;
        enemy.NoHealthLeft -= OnNoHealthLeft;
        
        base.ActionOnRelease(enemy);
        
        EnemyWasReleased?.Invoke(enemy);
        
        enemy.SetColliderCenter(_offsetAfterDeath, true);
    }
    
    private void OnNoHealthLeft(Enemy enemy)
    {
        _coinSpawner.Spawn(enemy.transform.position, enemy.CurrentStats.MoneyForKill);
        _crystalSpawner.Spawn(enemy.transform.position, enemy.CurrentStats.XpForKill);
        enemy.SetColliderCenter(_offsetAfterDeath, false);
    }
}
