using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelHandler : MonoBehaviour, IInteractableHandler
{
    [SerializeField] private InteractableHandlerBase _base;
    [SerializeField] private PickablesSpawner _coinSpawner;
    [SerializeField] private PickablesSpawner _xpSpawner;

    private int _minPickablesAmount = 2;
    private int _maxPickablesAmount = 8;
    private IPlayer _player;

    public int InitialCount => _base.InitialCount;

    public void Init(IPlayer player)
    {
        _player = player;
    }

    private void OnEnable()
    {
        _base.HandleChosenObject += OnInteractableChosen;
    }

    private void OnDisable()
    {
        _base.HandleChosenObject -= OnInteractableChosen;
    }

    public void Spawn(Vector3 position)
    {
        _base.Spawn(position);
    }
    
    private void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not Barrel barrel)
            throw new Exception("Barrel handler should handle barrels!");

        var coinsAmount = Random.Range(_minPickablesAmount, _maxPickablesAmount);
        var xpAmount = Random.Range(_minPickablesAmount, _maxPickablesAmount);
        
        _coinSpawner.Spawn(barrel.CurrentPosition, coinsAmount);
        _xpSpawner.Spawn(barrel.CurrentPosition, xpAmount);
    }
}
