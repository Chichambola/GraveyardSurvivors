using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using Random = UnityEngine.Random;

public class ItemsHandler : MonoBehaviour
{
    [SerializeField] private ItemSpawner _itemSpawner;
    [SerializeField] private List<Item> _items;
    
    private List<Item> _commonItems;
    private List<Item> _rareItems;
    private List<Item> _legendaryItems;
    private Dictionary<ERarityLevel, List<Item>> _itemsLists;

    private void Awake()
    {
        _commonItems = new List<Item>();
        _rareItems = new List<Item>();
        _legendaryItems = new List<Item>();
    }

    private void OnEnable()
    {
        SetItemsList();
    }

    public void SpawnRandomItem(Vector3 position, ERarityLevel rarity)
    {
        if (_itemsLists == null)
            throw new Exception();
        
        Item tempItem = UserUtils.GetElementByWeight(_itemsLists[rarity]);

        if (tempItem == null)
            throw new Exception($"{tempItem} is not a valid item");
        
        _itemSpawner.SetPrefab(tempItem);
        _itemSpawner.Spawn(position);
    }

    public IItem GetRandomItem()
    {
        int randomKeyIndex = Random.Range(0, _itemsLists.Keys.Count);
        
        var items = _itemsLists.ElementAt(randomKeyIndex).Value;

        int randomIndex = Random.Range(0, items.Count);

        return items[randomIndex];
    }
    
    private void SetItems()
    {
        foreach (var item in _items)
        {
            if (item.Rarity == ERarityLevel.Common)
                _commonItems.Add(item); 

            if (item.Rarity == ERarityLevel.Rare)
                _rareItems.Add(item);

            if (item.Rarity == ERarityLevel.Legendary)
                _legendaryItems.Add(item);
        }

        _itemsLists = new Dictionary<ERarityLevel, List<Item>>()
        {
            {ERarityLevel.Common,  _commonItems},
            {ERarityLevel.Rare, _rareItems},
            {ERarityLevel.Legendary, _legendaryItems}
        };
    }
    
    private void SetItemsList()
    {
        if (_items.Count != 0)
        {
            SetItems();
        }
        else
        {
            throw new Exception("No items!");
        }
    }
}
