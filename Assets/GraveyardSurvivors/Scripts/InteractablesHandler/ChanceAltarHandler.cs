using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class ChanceAltarHandler : MonoBehaviour, IPriceOwner, IInteractableHandler
{
    [SerializeField] private InteractableHandlerBase _base;
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private CostHandler _costHandler;
    
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
    
    public void InitializePrices() => _base.SetValueForObjects(_costHandler.Value);

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
