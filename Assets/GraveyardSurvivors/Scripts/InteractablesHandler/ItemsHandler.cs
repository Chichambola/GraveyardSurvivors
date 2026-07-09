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
    [SerializeField] private SerializableDictionary<Item, ERarityLevel> _itemsToDropPrefabs;
    [SerializeField] private SerializableDictionary<Item, ERarityLevel> _itemsForLevelUpPrefabs;
    [SerializeField] private List<Weapon> _weapons;

    private Dictionary<Item, ERarityLevel> _itemsToDrop;
    private Dictionary<Item, ERarityLevel> _itemsForLevelUp;

    private void Awake()
    {
        _itemsToDrop = _itemsToDropPrefabs.ToDictionary(item => item.Key, item => item.Value);
        _itemsForLevelUp = _itemsForLevelUpPrefabs.ToDictionary(item => item.Key, item => item.Value);
    }

    public void SpawnRandomItem(Vector3 position, ERarityLevel rarity)
    {
        var tempItems = _itemsToDrop.GetItemsByRarity(rarity);
        
        Item tempItem = UserUtils.GetElementByWeight(tempItems);

        if (tempItem == null)
            throw new Exception($"{tempItem} is not a valid item");
        
        _itemSpawner.SetPrefab(tempItem);
        _itemSpawner.Spawn(position);
    }
    
    public Item GetItemForLevelUp(ERarityLevel rarityLevel)
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
}
