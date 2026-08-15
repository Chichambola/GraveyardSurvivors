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
    [SerializeField] private List<InterfaceReference<IItem, MonoBehaviour>> _itemsToDropPrefabs;
    [SerializeField] private List<InterfaceReference<IItem, MonoBehaviour>> _itemsForLevelUpPrefabs;

    private List<IItem> _itemsToDrop;
    private List<IItem> _itemsForLevelUp;

    private void Awake()
    {
        _itemsToDrop = _itemsToDropPrefabs.Select(item => item.Value).ToList();
        _itemsForLevelUp = _itemsForLevelUpPrefabs.Select(item => item.Value).ToList();
    }

    private void OnEnable()
    {
        _itemsToDrop = _itemsToDrop.RemoveNonUniqueItems();
        _itemsForLevelUp = _itemsForLevelUp.RemoveNonUniqueItems();
    }

    public void SpawnRandomItem(Vector3 position, ERarityLevel rarity)
    {
        var tempItems = _itemsToDrop.GetWeightedItems(rarity);
        
        Item tempItem = UserUtils.GetElementByWeight(tempItems) as Item;

        if (tempItem == null)
            throw new Exception($"{tempItem} is not a valid item");
        
        _itemSpawner.SetPrefab(tempItem);
        _itemSpawner.Spawn(position);
    }

    public List<IItem> GetItemsForLevelUp() => _itemsForLevelUp.ToList();
}
