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

public class ChestHandler : MonoBehaviour, IPriceOwner, IInteractableHandler
{
    [SerializeField] private InteractableHandlerBase _base;
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private CostHandler _costHandler;
    [SerializeField] private TextMeshProUGUI _textCost;

    private IPlayer _player;

    public int InitialCount => _base.InitialCount;

    private void OnEnable()
    {
        _base.HandleChosenObject += OnInteractableChosen;
    }

    private void OnDisable()
    {
        _base.HandleChosenObject -= OnInteractableChosen;
    }

    public void Init(IPlayer player)
    {
        _player = player;
    }

    public void Spawn(Vector3 position)
    {
        _base.Spawn(position);
    }
    
    public void InitializePrices()
    {
        float value = _costHandler.Value;
        
        _textCost.text = $"{value}";
        
        _base.SetValueForObjects(value);
    }

    
    
    private void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not Chest chest)
            throw new Exception(nameof(chest));
        
        if (_player.MoneyAmount < _costHandler.Value)
        {
            Debug.Log("Not enough money");
        }
        else
        {
            chest.Open();
            
            _player.ReduceMoney(_costHandler.Value);
            
            RarityLevel rarityLevel = UserUtils.GetElementByWeight(chest.Weights);

            if (rarityLevel == null)
                throw new Exception(nameof(rarityLevel));
            
            _itemsHandler.SpawnRandomItem(chest.ItemStartPosition, rarityLevel.Rarity);
            
            _costHandler.Increase();

            _textCost.text = $"{_costHandler.Value}";
        }
        
        _base.SetValueForObjects(_costHandler.Value);
    }
}
