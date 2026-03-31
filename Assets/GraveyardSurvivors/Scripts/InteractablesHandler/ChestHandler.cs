using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ChestHandler : ItemInteractable
{
    private void OnEnable()
    {
        InteractableSpawner.InteractableWasChosen += OnChestChosen;
    }

    private void OnDisable()
    {
        InteractableSpawner.InteractableWasChosen -= OnChestChosen;
    }

    private void Start()
    {
        InteractableSpawner.SetValueForObjects(Cost);
    }

    private void OnChestChosen(Interactable interactable)
    {
        if (interactable is Chest chest == false)
            throw new Exception(nameof(chest));
        
        if (Player.MoneyAmount <= Cost)
        {
            Debug.Log("Not enough money");
        }
        else
        {
            chest.Open();
            
            Player.ReduceMoneyAmount(Cost);
        
            ERarityLevel rarityLevel = GetRarityLevel(chest);
            
            ItemsHandler.SpawnRandomItem(chest.transform.position, rarityLevel);
            
            IncreaseCost();
        }
        
        InteractableSpawner.SetValueForObjects(Cost);
    }
}
