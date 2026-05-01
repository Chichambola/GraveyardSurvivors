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
    [SerializeField] private Transform[] _points;
    [SerializeField] private Player _player;
    [SerializeField] private int _unitCost;
    [SerializeField] private int _weight;
    [SerializeField] private int _numberOfEnemiesSpawnAtOnce = 1;

    public event Action<Enemy> EnemyWasReleased;
    public event Action<Enemy> EnemyWasSpawned;
    
    private Vector3 _spawnPoint;
    private IntervalTimer _timer;
    
    public int Cost => _unitCost;
    public int Weight => _weight;

    public void Spawn()
    {
        for (int i = 0; i < _numberOfEnemiesSpawnAtOnce; i++)
        {
            _spawnPoint = GetRandomPoint();
            
            GetObject();
        }
    }
    
    protected override void ActionOnGet(Enemy enemy)
    {
        enemy.Init(_player);
        enemy.transform.position = _spawnPoint;
        enemy.transform.parent = transform;
        
        enemy.CanBeReleased += Release;
        
        ActiveObjects.Add(enemy);
        
        EnemyWasSpawned?.Invoke(enemy);
        
        base.ActionOnGet(enemy);
    }

    protected override void ActionOnRelease(Enemy enemy)
    {
        ActiveObjects.Remove(enemy);
        
        enemy.ResetCharacteristics();
        
        enemy.CanBeReleased -= Release;
        
        base.ActionOnRelease(enemy);
        
        EnemyWasReleased?.Invoke(enemy);
    }

    private Vector3 GetRandomPoint()
    {
        int randomIndex = Random.Range(0, _points.Length);
        
        return _points[randomIndex].position;
    }
}
