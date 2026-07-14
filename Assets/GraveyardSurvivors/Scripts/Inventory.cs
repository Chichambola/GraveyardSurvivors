using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    private Dictionary<Type, int> _items;
    private HashSet<Item> _createdItems;
    private int _startValue = 1;

    private void Awake()
    {
        _items = new Dictionary<Type, int>();
        _createdItems = new HashSet<Item>();
    }

    public void Add(Item item)
    {
        if (item == null)
            throw new Exception("Item cannot be null");
        
        if (!HasItem(item))
        {
            var tempItem = Create(item);

            if (tempItem is IAttackItem attackItem)
            {
                attackItem.SetPlayer(_player);
            }
        }
        else
        {
            if (TryGetItem(item, out var createdItem))
                return;

            if (createdItem is IUpgradeable upgradeable)
            {
                upgradeable.Upgrade();
            }
        }

        if (item is IBuff buff)
        {
            _player.AddBuff(buff);
        }
    }

    private bool TryGetItem(Item item, out Item createdItem)
    {
        var itemType = item.GetType();
            
        _items[itemType]++;

        createdItem = _createdItems.FirstOrDefault(i => i.GetType() == itemType);

        return createdItem == null;
    }

    private Item Create(Item item)
    {
        Item tempItem = Instantiate(item, transform);
        
        _createdItems.Add(tempItem);
        
        tempItem.Hide();

        var itemType = tempItem.GetType();
            
        _items.Add(itemType, _startValue);
        
        return tempItem;
    }

    private bool HasItem(Item item)
    {
        var type = item.GetType();

        var tempItem = _items.FirstOrDefault(i => i.Key == type);
        
        return tempItem.Key != null;
    }
}
