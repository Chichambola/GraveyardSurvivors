using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using Sherbert.Framework.Generic;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ItemsHandler : MonoBehaviour
{
    [SerializeField] private ItemSpawner _itemSpawner;
    [SerializeField] private SerializableDictionary<Item, ERarityLevel> _itemsToDropPrefabs;
    [SerializeField] private SerializableDictionary<Item, ERarityLevel> _itemsForLevelUpPrefabs;

    private Dictionary<Item, ERarityLevel> _itemsToDrop;
    private Dictionary<Item, ERarityLevel> _itemsForLevelUp;

    private void Awake()
    {
        _itemsToDrop = _itemsToDropPrefabs.ToDictionary(item => item.Key, item => item.Value);
        _itemsForLevelUp = _itemsForLevelUpPrefabs.ToDictionary(item => item.Key, item => item.Value);
    }

    public void SpawnRandomItem(Vector3 position, ERarityLevel rarity)
    {
        var tempItems = _itemsToDrop.GetWeightedObjects(rarity);
        
        Item tempItem = UserUtils.GetElementByWeight(tempItems) as Item;

        if (tempItem == null)
            throw new Exception($"{tempItem} is not a valid item");
        
        _itemSpawner.SetPrefab(tempItem);
        _itemSpawner.Spawn(position);
    }
    
    public Dictionary<Item, ERarityLevel> GetItemsForLevelUp()
    {
        return _itemsForLevelUp.ToDictionary(item => item.Key, item => item.Value);
    }
}
