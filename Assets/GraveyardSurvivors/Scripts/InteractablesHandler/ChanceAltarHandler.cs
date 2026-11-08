using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class ChanceAltarHandler : InteractableHandler, IPriceOwner, IInteractableHandler
{
    [SerializeField] private InteractableSpawner _interactableSpawner;
    [SerializeField] private int _initialSpawnCount = 10;
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private CostHandler _costHandler;
    
    private IPlayer _player;

    public int InitialCount => _initialSpawnCount;

    public void Init(IPlayer player)
    {
        _player = player;
    }
    
    private void OnValidate()
    {
        if (_initialSpawnCount < 0)
        {
            _initialSpawnCount = 0;
        }
    }


    private void OnEnable()
    {
        _interactableSpawner.InteractableWasChosen += OnInteractableChosen;
    }

    private void OnDisable()
    {
        _interactableSpawner.InteractableWasChosen -= OnInteractableChosen;
    }

    public void Spawn(Vector3 position)
    {
        _interactableSpawner.Spawn(position);
    }
    
    public void InitializePrices() => _interactableSpawner.SetValueForObjects(_costHandler.Value);

    private void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not ChanceAltar altar)
            throw new Exception(nameof(altar));
        
        if (_player.MoneyAmount < altar.CurrentCost)
        {
            Debug.Log($"Not enough money");
            
            return;
        }
        
        RarityLevel level = UserUtils.GetElementByWeight(altar.Weights);

        if (level == null)
            throw new Exception(nameof(level));

        _player.ReduceMoney(altar.CurrentCost);
        
        altar.StartCountdown();
        
        CanDrop(level.Rarity, altar);
            
        altar.Set(_costHandler.Increase(altar.CurrentCost));
    }
    
    private void CanDrop(ERarityLevel rarityLevel, ChanceAltar altar)
    {
        if (rarityLevel == ERarityLevel.None)
        {
            Debug.Log("Nothing to drop");   
        }
        else
        {
            altar.IncreaseInteractionsAmount();
            
            _itemsHandler.SpawnRandomItem(altar.ItemStartPosition, rarityLevel);   
        }
    }
}
