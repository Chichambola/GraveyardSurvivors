using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ChestHandler : InteractableHandler, IPriceOwner
{
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private CostHandler _costHandler;
    
    public void InitializePrices() => InteractableSpawner.SetValueForObjects(_costHandler.Cost);
    
    protected override void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not Chest chest)
            throw new Exception(nameof(chest));
        
        if (Player.MoneyAmount < _costHandler.Cost)
        {
            Debug.Log("Not enough money");
        }
        else
        {
            chest.Open();
            
            Player.ReduceMoney(_costHandler.Cost);
            
            RarityLevel rarityLevel = UserUtils.GetElementByWeight(chest.Weights);

            if (rarityLevel == null)
                throw new Exception(nameof(rarityLevel));
            
            _itemsHandler.SpawnRandomItem(chest.transform.position, rarityLevel.Rarity);
            
            _costHandler.IncreaseCost();
        }
        
        InteractableSpawner.SetValueForObjects(_costHandler.Cost);
    }
}
