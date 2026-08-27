using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AYellowpaper;
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
    private List<IItem> _itemsToShow;
    private List<IItem> _itemsList;
    private List<RarityLevel> _rarityLevels;

    private void Awake()
    {
        _tweenSettings = new TweenSettings<float>();
        _indexes = new List<int>();
        _itemsList = new List<IItem>();
        _itemsToShow = new List<IItem>();
        _rarityLevels = new List<RarityLevel>();
    }

    private void OnEnable()
    {
        _tweenSettings.settings.duration = _changeOpacityTime;
        _tweenSettings.settings.useUnscaledTime = true;
    }

    private void OnValidate()
    {
        _upgradeWindows = _upgradeWindows.RemoveNonUniqueItems();
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
        _itemsList = _itemsHandler.GetItemsForLevelUp();
        
        _rarityLevels.AddRange(_levels.Weights);
        
        for (int i = 0; i < _upgradeWindows.Count; i++)
        {
            bool isFound = false;
            
            while (isFound != true)
            {
                var level = UserUtils.GetElementByWeight(_rarityLevels);
                
                if (TryGetItem(level.Rarity, out IItem item))
                {
                    if (_itemsToShow.Contains(item))
                        continue;
                    
                    _itemsToShow.Add(item);
                        
                    _itemsList.Remove(item);
                    
                    isFound = true;
                }
                else
                {
                    if (_rarityLevels.Count <= 0)
                        throw new Exception($"You ran out of items");
                    
                    _rarityLevels.Remove(level);
                }
            }
        }
        
        _rarityLevels.Clear();
    }

    private bool TryGetItem(ERarityLevel rarity, out IItem item)
    {
        var items = _itemsList.GetWeightedItems(rarity);

        if (UserUtils.GetElementByWeight(items) is IItem tempItem)
        {
            item = tempItem;
            
            return true;
        }
        
        item = null;
        
        return false;
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
        
        _itemsList.Clear();

        if (upgrade is Item item)
        {
            ItemSelected?.Invoke(item);
            
            return;
        }
        
        if (upgrade is Weapon weapon)
        {
            WeaponSelected?.Invoke(weapon);
                
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
