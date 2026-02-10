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
    private Dictionary<RarityLevel, List<Item>> _itemsLists;

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

    private void SetItemsList()
    {
        if (_items.Count != 0)
        {
            foreach (var item in _items)
            {
                if (item.Info.Rarity == RarityLevel.Common)
                    _commonItems.Add(item); 

                if (item.Info.Rarity == RarityLevel.Rare)
                    _rareItems.Add(item);

                if (item.Info.Rarity == RarityLevel.Legendary)
                    _legendaryItems.Add(item);
            }

            _itemsLists = new Dictionary<RarityLevel, List<Item>>()
            {
                {RarityLevel.Common,  _commonItems},
                {RarityLevel.Rare, _rareItems},
                {RarityLevel.Legendary, _legendaryItems}
            };
        }
    }

    public void SpawnRandomItem(Vector3 position, RarityLevel rarity)
    {
        if (_itemsLists == null)
            throw new Exception();

        int firstIndex = 0;
        
        List<Item> desiredList = _itemsLists[rarity].ToList();

        int randomIndex = Random.Range(firstIndex, desiredList.Count - 1);
        
        _itemSpawner.SetPrefab(desiredList[randomIndex]);
        _itemSpawner.Spawn(position);
    }
}
