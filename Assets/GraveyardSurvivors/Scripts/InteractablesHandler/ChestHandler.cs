using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestHandler : ChanceHandlerBase
{
    [SerializeField] private ChestSpawner _chestSpawner;

    private void Start()
    {
        if(Cost <= 0)
            throw new Exception("Cost must be greater than 0.");
        
        SetObjectsValue();
    }
    
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
        
        if (Player.MoneyAmount <= CurrentCost)
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
            
            ItemsHandler.SpawnRandomItem(chest.transform.position, rarityLevel);
            
            CalculateCost();
        }
    }

    protected override void SetObjectsValue()
    {
        foreach (var chest in _chestSpawner.SpawnedObjects)
        {
            chest.SetValue(Cost);
        }
    }
}
