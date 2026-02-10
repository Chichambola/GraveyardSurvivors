using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner<Item>
{
    [SerializeField] private Thrower _thrower;
    
    private Vector3 _spawnPosition;
    
    public void Spawn(Vector3 position)
    {
        _spawnPosition = position;
        
        GetObject();
    }
    
    protected override void ActionOnGet(Item item)
    {
        item.transform.parent = transform.parent;
        item.transform.position = _spawnPosition;
        
        item.CanBeReleased += Release;
        
        base.ActionOnGet(item);
        
        _thrower.Throw(item);
    }

    protected override void ActionOnRelease(Item item)
    {
        item.CanBeReleased -= Release;
        
        base.ActionOnRelease(item);
    }
}
