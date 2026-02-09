using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private ChestSpawner _chestSpawner;

    private int _lowestPercent = 0;
    private int _highestPercent = 100;
    
    private int _commonChancePercent = 60;
    private int _rareChancePercent = 75;
    private int _legendaryChancePercent = 90;
    
    private void OnEnable()
    {
        _chestSpawner.ChestWasReleased += OnChestReleased;
    }

    private void OnDisable()
    {
        _chestSpawner.ChestWasReleased -= OnChestReleased;
    }

    private void OnChestReleased()
    {
        float percentChance = GetChance();

        Item item = GetItem(percentChance);
        
        Debug.Log(item);
    }

    private float GetChance()
    {
        float percentChance = Random.Range(_lowestPercent, _highestPercent);

        percentChance += _player.CurrentStats.Luck;

        if (percentChance > _highestPercent)
            percentChance = _highestPercent;
        
        return percentChance;
    }

    private Item GetItem(float value)
    {
        Item tempItem;
        
        if (value >= _lowestPercent && value <= _commonChancePercent)
        {
            return tempItem = _itemsHandler.GetItem(RarityLevel.Common);
        }

        if (value > _commonChancePercent && value <= _rareChancePercent)
        {
            return tempItem = _itemsHandler.GetItem(RarityLevel.Rare);
        }

        if (value > _legendaryChancePercent && value <= _highestPercent)
        {
            return tempItem = _itemsHandler.GetItem(RarityLevel.Legendary);
        }

        tempItem = null;
        
        return tempItem;
    }
}
