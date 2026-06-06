using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ChestHandler : CostInteractableHandler, IInteractableHandler
{
    [SerializeField] private ItemsHandler _itemsHandler;
    
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
        
        if (Player.MoneyAmount < Cost)
        {
            Debug.Log("Not enough money");
        }
        else
        {
            chest.Open();
            
            Player.ReduceMoney(Cost);
        
            chest.IncreaseChance(Player.Luck);
            
            RarityLevel rarityLevel = UserUtils.GetElementByWeight(chest.Weights.ToList());

            if (rarityLevel == null)
                throw new Exception(nameof(rarityLevel));
            
            _itemsHandler.SpawnRandomItem(chest.transform.position, rarityLevel.Rarity);
            
            IncreaseCost();
        }
        
        InteractableSpawner.SetValueForObjects(Cost);
    }
}
