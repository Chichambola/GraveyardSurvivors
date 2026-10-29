using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ChestHandler : InteractableHandler, IPriceOwner
{
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private CostHandler _costHandler;
    [SerializeField] private TextMeshProUGUI _textCost;
    
    public void InitializePrices()
    {
        float value = _costHandler.Value;
        
        _textCost.text = $"{value}";
        
        InteractableSpawner.SetValueForObjects(value);
    }

    protected override void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not Chest chest)
            throw new Exception(nameof(chest));
        
        if (Player.MoneyAmount < _costHandler.Value)
        {
            Debug.Log("Not enough money");
        }
        else
        {
            chest.Open();
            
            Player.ReduceMoney(_costHandler.Value);
            
            RarityLevel rarityLevel = UserUtils.GetElementByWeight(chest.Weights);

            if (rarityLevel == null)
                throw new Exception(nameof(rarityLevel));
            
            _itemsHandler.SpawnRandomItem(chest.ItemStartPosition, rarityLevel.Rarity);
            
            _costHandler.Increase();

            _textCost.text = $"{_costHandler.Value}";
        }
        
        InteractableSpawner.SetValueForObjects(_costHandler.Value);
    }
}
