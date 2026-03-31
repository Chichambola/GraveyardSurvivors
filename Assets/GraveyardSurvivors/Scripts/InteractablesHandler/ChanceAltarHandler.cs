using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class ChanceAltarHandler : ItemInteractable
{
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
        
        ERarityLevel rarityLevel = GetRarityLevel(altar, altar.NoneChance);

        Player.ReduceMoneyAmount(altar.CurrentCost);
        
        altar.StartCountdown();
        
        CanDrop(rarityLevel, altar);
            
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
            
            ItemsHandler.SpawnRandomItem(altar.transform.position, rarityLevel);   
        }
    }
}
