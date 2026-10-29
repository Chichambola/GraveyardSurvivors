using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class ChanceAltarHandler : InteractableHandler, IPriceOwner
{
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private CostHandler _costHandler;

    public void InitializePrices() => InteractableSpawner.SetValueForObjects(_costHandler.Value);

    protected override void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not ChanceAltar altar)
            throw new Exception(nameof(altar));
        
        if (Player.MoneyAmount < altar.CurrentCost)
        {
            Debug.Log($"Not enough money");
            
            return;
        }
        
        RarityLevel level = UserUtils.GetElementByWeight(altar.Weights);

        if (level == null)
            throw new Exception(nameof(level));

        Player.ReduceMoney(altar.CurrentCost);
        
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
