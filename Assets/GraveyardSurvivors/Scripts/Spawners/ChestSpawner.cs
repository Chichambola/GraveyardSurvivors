using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : Spawner<Chest>
{
    [SerializeField] private Transform _point;

    public event Action<Chest> ChestWasReleased;
    
    private void OnEnable()
    {
        GetObject();
    }

    protected override void ActionOnGet(Chest chest)
    {
        chest.transform.position = _point.transform.position;

        chest.CanBeReleased += Release;
        
        base.ActionOnGet(chest);
    }

    protected override void ActionOnRelease(Chest chest)
    {
        chest.CanBeReleased += Release;
        
        base.ActionOnRelease(chest);
        
        ChestWasReleased?.Invoke(chest);
    }
}
