using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlaceholderSpawner : Spawner<ItemPlaceholder>
{
    public event Action<Vector3> ItemStoppedMoving;
    
    private Vector3 _spawnPosition;

    public void Spawn(Vector3 position)
    {
        _spawnPosition = position;
        
        GetObject();
    }
    
    protected override void ActionOnGet(ItemPlaceholder placeholder)
    {
        base.ActionOnGet(placeholder);
        
        placeholder.transform.parent = transform;
        placeholder.transform.position = _spawnPosition;
        placeholder.CanBeReleased += Release;

        ActiveObjects.Add(placeholder);
        
        placeholder.StartMoving();
    }

    protected override void ActionOnRelease(ItemPlaceholder placeholder)
    {
        placeholder.CanBeReleased -= Release;
        ItemStoppedMoving?.Invoke(placeholder.transform.position);
        
        ActiveObjects.Remove(placeholder);
        
        placeholder.ResetCharacteristics();
        
        base.ActionOnRelease(placeholder);
    }
}
