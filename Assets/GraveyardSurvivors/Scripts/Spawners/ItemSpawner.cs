using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    }

    protected override void ActionOnRelease(Item item)
    {
        item.CanBeReleased -= Release;
        item.ResetCharacteristics();
        
        ActiveObjects.Remove(item);
        
        Destroy(item.gameObject);
    }

    private void OnItemStoppedMoving(Vector3 position)
    {
        _spawnPosition = position;

        var item = Instantiate(ObjectPrefab);
        
        ActionOnGet(item);
    }
}
