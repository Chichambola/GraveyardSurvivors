using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner<Item>
{
    [SerializeField] private PlaceholderSpawner _placeholderSpawner;
    
    private Vector3 _spawnPosition;
    private List<Item> _spawnedObjects;
    
    private void OnEnable()
    {
        _spawnedObjects = new List<Item>();
        
        _placeholderSpawner.ItemStoppedMoving += OnItemStoppedMoving;
    }

    private void OnDisable()
    {
        _placeholderSpawner.ItemStoppedMoving -= OnItemStoppedMoving;
        _spawnedObjects.Clear();
    }

    public void Spawn(Vector3 position)
    {
        _placeholderSpawner.Spawn(position);
    }
    
    protected override void ActionOnGet(Item item)
    {
        item.transform.position = _spawnPosition;
        item.transform.parent = transform;

        item.CanBeReleased += Release;
        
        ActiveObjects.Add(item);
        
        base.ActionOnGet(item);
    }

    protected override void ActionOnRelease(Item item)
    {
        item.CanBeReleased -= Release;
        
        ActiveObjects.Remove(item);
        
        base.ActionOnRelease(item);
    }

    private void OnItemStoppedMoving(Vector3 position)
    {
        _spawnPosition = position;
        
        GetObject();
    }
}
