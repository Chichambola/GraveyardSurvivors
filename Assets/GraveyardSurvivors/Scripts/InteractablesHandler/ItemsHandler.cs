using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sherbert.Framework.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ItemsHandler : MonoBehaviour
{
    [SerializeField] private ItemSpawner _itemSpawner;
    [SerializeField] private SerializableDictionary<ItemSettings, Item> _itemsPrefabs;
    [SerializeField] private List<Weapon> _weapons;

    private Dictionary<ERarityLevel, Item> _itemsToDrop;
    private Dictionary<ERarityLevel, Item> _itemsForLevelUp;

    private void Awake()
    {
        _itemsToDrop = new ();
        _itemsForLevelUp = new ();
    }

    private void OnEnable()
    {
        SetItems();
        
        foreach (var weapon in _weapons)
        {
            weapon.Init();
        }
    }

    public void SpawnRandomItem(Vector3 position, ERarityLevel rarity)
    {
        if (_itemsPrefabs == null)
            throw new Exception();

        var tempItems = _itemsToDrop.GetItemsByRarity(rarity);
        
        Item tempItem = UserUtils.GetElementByWeight(tempItems);

        if (tempItem == null)
            throw new Exception($"{tempItem} is not a valid item");
        
        _itemSpawner.SetPrefab(tempItem);
        _itemSpawner.Spawn(position);
    }
    
    public Item GetRandomItem(ERarityLevel rarityLevel)
    {
        var tempItems = _itemsForLevelUp.GetItemsByRarity(rarityLevel);
        
        Item tempItem = UserUtils.GetElementByWeight(tempItems);
        
        return tempItem;
    }
    
    public Weapon GetRandomWeapon()
    {
        int randomIndex = Random.Range(0, _weapons.Count);
        
        return _weapons[randomIndex];
    }
    
    private void SetItems()
    {
        foreach (var setting in _itemsPrefabs.Keys)
        {
            if (setting.WaysOfObtaining == EWaysOfObtaining.ByDropping)
            {
                _itemsToDrop.Add(setting.Rarity, _itemsPrefabs[setting]);
            }
            else
            {
                _itemsForLevelUp.Add(setting.Rarity, _itemsPrefabs[setting]);
            }
        }
    }
}
