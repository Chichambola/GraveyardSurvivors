using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Sherbert.Framework.Generic;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradesHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup _background;
    [SerializeField] private SerializableDictionary<ERarityLevel, Color> _rarityColors;
    
    [Header("Upgrade windows and settings")]
    [SerializeField] private List<UpgradeWindowButton> _upgradeWindows;
    [SerializeField] private float _changeOpacityTime = 0.5f;
    
    [Header("Items handler and its values")]
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private RarityLevelHandler _levels;
    [SerializeField] private int _searchCount = 10;
    
    public event Action<Item> ItemSelected;
    public event Action<Weapon> WeaponSelected;
    
    private float _fullVisibility = 1;
    private float _fullOpacity = 0;
    private TweenSettings<float> _tweenSettings;
    private IPlayer _player;
    private List<int> _indexes;
    private List<Item> _itemsToShow;
    private Dictionary<Item, ERarityLevel> _itemsDict;

    private void Awake()
    {
        _tweenSettings = new TweenSettings<float>();
        _indexes = new List<int>();
        _itemsDict = new Dictionary<Item, ERarityLevel>();
        _itemsToShow = new List<Item>();
    }

    private void OnEnable()
    {
        _tweenSettings.settings.duration = _changeOpacityTime;
        _tweenSettings.settings.useUnscaledTime = true;
    }
    
    public void SetPlayer(IPlayer player)
    {
        _player = player ?? throw new ArgumentNullException(nameof(player));
    }
    
    public void ShowUpgrades()
    {
        _indexes.Clear();
        
        _indexes = _upgradeWindows.Select(upgradeWindow => _upgradeWindows.IndexOf(upgradeWindow)).ToList();
        
        ChangeBackgroundOpacity(_fullVisibility);
        
        FindItems();
        
        SetWindows();
        
        foreach (var upgradeWindow in _upgradeWindows)
        {
            upgradeWindow.SetSettings(true);
            
            upgradeWindow.ChangeOpacity(_fullOpacity, _fullVisibility, _changeOpacityTime);

            upgradeWindow.UpgradeSelected += OnUpgradeSelected;
        }
    }

    private void FindItems()
    {
        _itemsDict = _itemsHandler.GetItemsForLevelUp();
        
        for (int i = 0; i < _upgradeWindows.Count; i++)
        {
            bool isFound = false;

            int searchAmount = 0;
            
            while (isFound != true || searchAmount > _searchCount)
            {
                var item = GetItem(_itemsDict);

                if (!_itemsToShow.Contains(item))
                {
                    _itemsToShow.Add(item);
                        
                    isFound = true;

                    if (item != null)
                        _itemsDict.Remove(item);
                    else
                        throw new Exception(nameof(item));
                }

                searchAmount++;
            }

            if (searchAmount > _searchCount)
            {
                var item = GetItem(_itemsDict);
                
                _itemsToShow.Add(item);
            }
        }
    }

    private Item GetItem(Dictionary<Item, ERarityLevel> itemsDict)
    {
        RarityLevel level = UserUtils.GetElementByWeight(_levels.Weights);
            
        var items = itemsDict.GetWeightedObjects(level.Rarity);

        var item = UserUtils.GetElementByWeight(items) as Item;

        if (item == null)
            throw new Exception($"{nameof(item)} is not Item");
        
        item.SetRarityLevel(level);

        return item;

    }

    private void ChangeBackgroundOpacity(float alphaValue)
    {
        _tweenSettings.endValue = alphaValue;

        Tween.Alpha(_background, _tweenSettings);
    }

    private void OnUpgradeSelected(IItem upgrade)
    {
        foreach (var upgradeWindow in _upgradeWindows)
        {
            upgradeWindow.UpgradeSelected -= OnUpgradeSelected;
            upgradeWindow.SetSettings(false);
        }
        
        ChangeBackgroundOpacity(_fullOpacity);
        
        _itemsToShow.Clear();
        
        _itemsDict.Clear();

        if (upgrade is Item item)
        {
            if (upgrade is Weapon weapon)
            {
                WeaponSelected?.Invoke(weapon);
                
                return;
            }
            
            ItemSelected?.Invoke(item);
            
            return;
        }
    }
    
    private void SetWindows()
    {
        foreach (var item in _itemsToShow)
        {
            if (item is Weapon weapon)
            {
                bool hasWeapon = _player.HasWeapon(weapon);
            
                weapon.SetDescription(hasWeapon ? weapon.UpgradeDescription : weapon.BaseDescription);
            }
            
            int randomIndex = Random.Range(0, _indexes.Count);
        
            int index = _indexes[randomIndex];
            
            _indexes.Remove(index);
        
            _upgradeWindows[index].SetWindow(item, _rarityColors[item.Rarity]);
        }
    }
}
