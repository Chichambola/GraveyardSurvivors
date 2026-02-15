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
    
    private void OnChanceAltarChosen(ChanceAltar altar)
    {
        int commonChance = altar.CommonChance;
        int rareChance = altar.RareChance;
        int legendaryChance = altar.LegendaryChance;
        
        ERarityLevel rarityLevel = RarityEvaluator.GetRarityLevel(commonChance, rareChance, legendaryChance, out float currentPercent);

        if (Player.HasEnoughMoney(altar.CurrentCost) == false)
        {
            Debug.Log($"Not enough money");
            
            return;
        }

        if (currentPercent < commonChance)
        {
            Debug.Log("Nothing to drop");   
        }
        else
        {
            altar.IncreaseInteractionsAmount();
            
            ItemsHandler.SpawnRandomItem(altar.CurrentPoints, rarityLevel);   
        }


        Player.ReduceMoneyAmount(altar.CurrentCost);
    }
}
