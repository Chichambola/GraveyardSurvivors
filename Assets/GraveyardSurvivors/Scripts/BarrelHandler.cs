using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelHandler : InteractableHandler
{
    [SerializeField] private PickablesSpawner _coinSpawner;
    [SerializeField] private PickablesSpawner _xpSpawner;

    private int _minPickablesAmount = 2;
    private int _maxPickablesAmount = 8;
    
    protected override void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not Barrel barrel)
            throw new Exception("Barrel handler should handle barrels!");

        var coinsAmount = Random.Range(_minPickablesAmount, _maxPickablesAmount);
        var xpAmount = Random.Range(_minPickablesAmount, _maxPickablesAmount);
        
        _coinSpawner.Spawn(barrel.CurrentPosition, coinsAmount);
        _xpSpawner.Spawn(barrel.CurrentPosition, xpAmount);
    }
}
