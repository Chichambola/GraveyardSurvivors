using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.Rendering;

public class EnemySpawner : Spawner<Enemy>
{
    [SerializeField] private Transform _point;
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        Spawn();
    }

    public void Spawn()
    {
        GetObject();
    }
    
    protected override void ActionOnGet(Enemy enemy)
    {
        enemy.Init(_player);
        enemy.CanBeReleased += Release;
        
        ActiveObjects.Add(enemy);
        
        base.ActionOnGet(enemy);
    }

    protected override void ActionOnRelease(Enemy enemy)
    {
        enemy.CanBeReleased -= Release;
        enemy.ResetCharacteristics();
        
        ActiveObjects.Remove(enemy);
        
        base.ActionOnRelease(enemy);
    }
}
