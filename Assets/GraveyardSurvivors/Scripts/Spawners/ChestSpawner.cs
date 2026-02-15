using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : Spawner<Chest>
{
    [SerializeField] private Transform _point;

    public event Action<Chest> ChestWasReleased;
    public event Action<Chest> ChestWasChosen;
    
    private void OnEnable()
    {
        GetObject();
    }

    protected override void ActionOnGet(Chest chest)
    {
        chest.transform.position = _point.transform.position;
        chest.transform.parent = transform;

        chest.CanBeReleased += Release;
        chest.WasChosen += OnChestChosen; 
        
        base.ActionOnGet(chest);
    }

    protected override void ActionOnRelease(Chest chest)
    {
        chest.CanBeReleased -= Release;
        chest.WasChosen -= OnChestChosen; 
        
        base.ActionOnRelease(chest);
        
        ChestWasReleased?.Invoke(chest);
    }

    private void OnChestChosen(Chest chest)
    {
        ChestWasChosen?.Invoke(chest);
    }
}
