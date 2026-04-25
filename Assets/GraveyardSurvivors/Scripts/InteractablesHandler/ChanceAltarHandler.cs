using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class ChanceAltarHandler : ItemInteractable
{
    [SerializeField] private ItemsHandler _itemsHandler;
    
    private void OnEnable()
    {
        InteractableSpawner.InteractableWasChosen += OnChanceAltarChosen;
    }

    private void OnDisable()
    {
        InteractableSpawner.InteractableWasChosen -= OnChanceAltarChosen;
    }

    private void Start()
    {
        InteractableSpawner.SetValueForObjects(Cost);
    }

    private void OnChanceAltarChosen(Interactable interactable)
    {
        if (interactable is ChanceAltar altar == false)
            throw new Exception();
        
        if (Player.MoneyAmount <= altar.CurrentCost)
        {
            Debug.Log($"Not enough money");
            
            return;
        }
        
        RarityLevel rarityLevel = UserUtils.GetElementByWeight(altar.Weights) as RarityLevel;

        if (rarityLevel == null)
            throw new Exception(nameof(rarityLevel));

        Player.ReduceMoneyAmount(altar.CurrentCost);
        
        altar.StartCountdown();
        
        CanDrop(rarityLevel.Rarity, altar);
            
        altar.SetValue(IncreaseCost(altar.CurrentCost));
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
}
