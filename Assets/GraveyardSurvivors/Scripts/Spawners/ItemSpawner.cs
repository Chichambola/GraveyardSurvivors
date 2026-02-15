using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner<Item>
{
    [SerializeField] private PlaceholderSpawner _placeholderSpawner;
    
    private Vector3 _spawnPosition;

    private void OnEnable()
    {
        _placeholderSpawner.ItemStoppedMoving += OnItemStoppedMoving;
    }

    private void OnDisable()
    {
        _placeholderSpawner.ItemStoppedMoving -= OnItemStoppedMoving;
    }

    public void Spawn(QuadraticCurvePoints points)
    {
        _placeholderSpawner.Spawn(points);
    }
    
    protected override void ActionOnGet(Item item)
    {
        item.transform.position = _spawnPosition;
        item.transform.parent = transform;

        item.CanBeReleased += Release;
        
        base.ActionOnGet(item);
    }

    protected override void ActionOnRelease(Item item)
    {
        item.CanBeReleased -= Release;
        
        base.ActionOnRelease(item);
    }

    private void OnItemStoppedMoving(Vector3 position)
    {
        _spawnPosition = position;
        
        GetObject();
    }
}
