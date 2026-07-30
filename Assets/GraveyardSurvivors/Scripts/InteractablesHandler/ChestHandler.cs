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
    
    private void OnEnable()
    {
        InteractableSpawner.InteractableWasChosen += OnChestChosen;
    }

    private void OnDisable()
    {
        InteractableSpawner.InteractableWasChosen -= OnChestChosen;
    }

    private void OnChestChosen(Interactable interactable)
    {
        if (interactable is Chest chest == false)
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
    
    public void InitializePrices() => InteractableSpawner.SetValueForObjects(_costHandler.Cost);
}
