using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.Rendering;

public class EnemySpawner : Spawner<Enemy>, IEnemySpawner<Enemy>, IWeightedObject
{
    [SerializeField] private CoinSpawner _coinSpawner;
    [SerializeField] private Transform[] _points;
    [SerializeField] private Player _player;
    [SerializeField] private int _unitCost;
    [SerializeField] private int _weight;

    public event Action<Enemy> EnemyWasReleased;
    
    private Vector3 _spawnPoint;
    
    public int Cost => _unitCost;
    public int Weight => _weight;

    public void Spawn()
    {
        foreach (var point in _points)
        {
            _spawnPoint = point.position;
            
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
        
        base.ActionOnGet(enemy);
    }

    protected override void ActionOnRelease(Enemy enemy)
    {
        _coinSpawner.Spawn(enemy.transform.position, enemy.CurrentStats.MoneyForKill);
        
        enemy.CanBeReleased -= Release;
        EnemyWasReleased?.Invoke(enemy);
        
        enemy.ResetCharacteristics();
        
        ActiveObjects.Remove(enemy);
        
        base.ActionOnRelease(enemy);
    }
}
