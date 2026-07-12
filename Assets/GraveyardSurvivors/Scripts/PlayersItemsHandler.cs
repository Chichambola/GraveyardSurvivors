using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayersItemsHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    private Dictionary<Item, int> _items;
    private int _startValue = 1;

    private void Awake()
    {
        _items = new Dictionary<Item, int>();
    }

    public void AddItem(Item item)
    {
        if (item == null)
            throw new Exception("Item cannot be null");
        
        if (HasItem(item))
        {
            Item tempItem = Instantiate(item, transform);
            
            if (tempItem is IAttackItem attackItem)
            {
                attackItem.SetPlayer(_player);
            }
        }
        else
        {
            _items[item]++;
            
            if (item is IUpgradeable upgradeable)
            {
                upgradeable.Upgrade();
            }
        }

        if (item is IBuff buff)
        {
            _player.AddBuff(buff);
        }
    }

    public bool HasItem(Item item)
    {
        var type = item.GetType();

        var tempItem = _items.FirstOrDefault(i => i.GetType() == type);
        
        return tempItem.Key != null;
    }
}
