using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestHandler : MonoBehaviour
{
    [SerializeField] private Thrower _thrower;
    [SerializeField] private RarityEvaluator _rarityEvalutor;
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private ChestSpawner _chestSpawner;
    
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

    private void OnChestReleased(Chest chest)
    {
        RarityLevel rarityLevel = _rarityEvalutor.GetRarityLevel(_commonChancePercent, _rareChancePercent, _legendaryChancePercent);
        
        _itemsHandler.SpawnRandomItem(chest.transform.position, chest.Points, rarityLevel);
    }
}
