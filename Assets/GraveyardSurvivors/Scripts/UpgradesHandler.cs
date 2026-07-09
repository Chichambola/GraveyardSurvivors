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
    [SerializeField] private int _amountOfWeaponsPerUpgrade = 1;
    [SerializeField] private RarityLevelHandler _levels;
    
    public event Action<Item> ItemSelected;
    public event Action<Weapon> WeaponSelected;
    
    private float _fullVisibility = 1;
    private float _fullOpacity = 0;
    private int _amountOfItemWindows;
    private TweenSettings<float> _tweenSettings;
    private ItemSettings _itemSettings;
    private IPlayer _player;
    private List<int> _indexes;

    private void Awake()
    {
        _tweenSettings = new TweenSettings<float>();
        _indexes = new List<int>();
        _itemSettings = new ItemSettings();
    }

    private void OnValidate()
    {
        if (_amountOfWeaponsPerUpgrade >= _upgradeWindows.Count)
        {
            _amountOfWeaponsPerUpgrade = _upgradeWindows.Count - 1;
        }

        if (_amountOfWeaponsPerUpgrade < 0)
        {
            _amountOfWeaponsPerUpgrade = 0;
        }
    }

    private void OnEnable()
    {
        _tweenSettings.settings.duration = _changeOpacityTime;
        _tweenSettings.settings.useUnscaledTime = true;

        _amountOfItemWindows = _upgradeWindows.Count - _amountOfWeaponsPerUpgrade;
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
        
        SetWindowsWithWeapons();
        
        SetWindowsWithItems();

        foreach (var upgradeWindow in _upgradeWindows)
        {
            upgradeWindow.SetSettings(true);
            
            upgradeWindow.ChangeOpacity(_fullOpacity, _fullVisibility, _changeOpacityTime);

            upgradeWindow.UpgradeSelected += OnUpgradeSelected;
        }
    }

    private void SetWindowsWithItems()
    {
        for (int i = 0; i < _amountOfItemWindows; i++)
        {
            RarityLevel level = UserUtils.GetElementByWeight(_levels.Weights);
            
            var item = _itemsHandler.GetItemForLevelUp(level.Rarity);

            _itemSettings.Rarity = level.Rarity;
            _itemSettings.Color = _rarityColors[level.Rarity];
            
            SetRandomWindow(item);
        }
    }

    private void SetWindowsWithWeapons()
    {
        for (int i = 0; i < _amountOfWeaponsPerUpgrade; i++)
        {
            var weapon = _itemsHandler.GetRandomWeapon();

            bool hasWeapon = PlayerHandler.HasPlayerWeapon(weapon);
            
            weapon.SetDescription(hasWeapon ? weapon.UpgradeDescription : weapon.BaseDescription);
            
            _itemSettings.Color = _rarityColors[ERarityLevel.None];
            _itemSettings.Rarity = ERarityLevel.None;
            
            SetRandomWindow(weapon);
        }
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
    
    private void SetRandomWindow(IItem item)
    {
        int randomIndex = Random.Range(0, _indexes.Count);
        
        int index = _indexes[randomIndex];
        
        _indexes.Remove(index);
        
        _upgradeWindows[index].SetWindow(item, _itemSettings);
    }
}
