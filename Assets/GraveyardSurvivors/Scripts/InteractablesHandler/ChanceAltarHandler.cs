using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceAltarHandler : ChanceHandlerBase
{
    [SerializeField] private ChanceAltarSpawner _chanceAltarSpawner;

    private void OnEnable()
    {
        _chanceAltarSpawner.AltarWasChosen += OnChanceAltarChosen;
    }

    private void OnDisable()
    {
        _chanceAltarSpawner.AltarWasChosen -= OnChanceAltarChosen;
    }
    
    private void Start()
    {
        if(Cost <= 0)
            throw new Exception("Cost must be greater than 0.");
        
        SetObjectsValue();
    }

    protected override void SetObjectsValue()
    {
        foreach (var altar in _chanceAltarSpawner.SpawnedObjects)
        {
            altar.SetCost(Cost);
        }
    }

    private void OnChanceAltarChosen(ChanceAltar altar)
    {
        int noneChance = altar.NoneChance;
        int commonChance = altar.CommonChance;
        int rareChance = altar.RareChance;
        int legendaryChance = altar.LegendaryChance;
        
        ERarityLevel rarityLevel = RarityEvaluator.GetRarityLevel(noneChance, commonChance, rareChance, legendaryChance);

        if (Player.MoneyAmount <= altar.CurrentCost)
        {
            Debug.Log($"Not enough money");
            
            return;
        }

        Player.ReduceMoneyAmount(altar.CurrentCost);
        altar.StartCountdown();
        
        Debug.Log(rarityLevel);
        
        if (rarityLevel == ERarityLevel.None)
        {
            Debug.Log("Nothing to drop");   
        }
        else
        {
            altar.IncreaseInteractionsAmount();
            
            ItemsHandler.SpawnRandomItem(altar.transform.position, rarityLevel);   
        }
        
        CalculateCost();
        altar.SetCost(Cost);
    }
}
