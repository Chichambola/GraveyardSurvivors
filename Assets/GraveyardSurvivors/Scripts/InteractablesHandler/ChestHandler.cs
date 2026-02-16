using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestHandler : ChanceHandlerBase
{
    [SerializeField] private ChestSpawner _chestSpawner;

    private void OnEnable()
    {
        _chestSpawner.ChestWasChosen += OnChestChosen;
    }

    private void OnDisable()
    {
        _chestSpawner.ChestWasChosen -= OnChestChosen;
    }

    private void OnChestChosen(Chest chest)
    {
        if (chest == null)
            throw new Exception(nameof(chest));
        
        if (Player.HasEnoughMoney(CurrentCost) == false)
        {
            Debug.Log("Not enough money");
        }
        else
        {
            chest.Open();
            
            Player.ReduceMoneyAmount(CurrentCost);
            
            int commonChance = chest.CommonChance;
            int rareChance = chest.RareChance;
            int legendaryChance = chest.LegendaryChance;
        
            ERarityLevel rarityLevel = RarityEvaluator.GetRarityLevel(commonChance, rareChance, legendaryChance);
        
            ItemsHandler.SpawnRandomItem(chest.CurrentPoints, rarityLevel);
            
            CalculateCost();
        }
    }
}
