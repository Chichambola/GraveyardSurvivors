using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class ChanceAltarHandler : InteractableHandler, IPriceOwner
{
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private CostHandler _costHandler;
    
    private void OnEnable()
    {
        InteractableSpawner.InteractableWasChosen += OnChanceAltarChosen;
    }

    private void OnDisable()
    {
        InteractableSpawner.InteractableWasChosen -= OnChanceAltarChosen;
    }

    private void OnChanceAltarChosen(Interactable interactable)
    {
        if (interactable is ChanceAltar altar == false)
            throw new Exception();
        
        if (Player.MoneyAmount < altar.CurrentCost)
        {
            Debug.Log($"Not enough money");
            
            return;
        }
        
        RarityLevel rarityLevel = UserUtils.GetElementByWeight(altar.Weights) as RarityLevel;

        if (rarityLevel == null)
            throw new Exception(nameof(rarityLevel));

        Player.ReduceMoney(altar.CurrentCost);
        
        altar.StartCountdown();
        
        CanDrop(rarityLevel.Rarity, altar);
            
        altar.SetValue(_costHandler.IncreaseCost(altar.CurrentCost));
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
            
            _itemsHandler.SpawnRandomItem(altar.transform.position, rarityLevel);   
        }
    }

    public void InitializePrices() => InteractableSpawner.SetValueForObjects(_costHandler.Cost);
}
