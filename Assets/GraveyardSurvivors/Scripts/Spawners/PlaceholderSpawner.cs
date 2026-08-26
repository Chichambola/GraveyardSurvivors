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
        placeholder.SetPosition(_spawnPosition);
        placeholder.CanBeReleased += Release;
        ActiveObjects.Add(placeholder);
        
        base.ActionOnGet(placeholder);
        
       placeholder.StartThrowing();
    }

    protected override void ActionOnRelease(ItemPlaceholder placeholder)
    {        
        ItemStoppedMoving?.Invoke(placeholder.transform.position);
        
        placeholder.transform.parent = transform;
        
        placeholder.CanBeReleased -= Release;
        
        ActiveObjects.Remove(placeholder);
        
        placeholder.ResetCharacteristics();
        
        base.ActionOnRelease(placeholder);
    }
}
