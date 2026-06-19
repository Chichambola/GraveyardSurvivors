using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradesHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup _background;
    [Header("Upgrade windows and settings")]
    [SerializeField] private List<UpgradeWindow> _upgradeWindows;
    [SerializeField] private float _changeOpacityTime = 0.5f;
    [Header("Items handler and its values")]
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private int _amountOfWeaponsPerUpgrade = 1;
    
    public event Action<IItem> UpgradeSelected; 
    
    private float _normalTimeSpeed = 1;
    private float _pauseTime = 0.00001f;
    private float _fullVisibility = 1;
    private float _fullOpacity = 0;
    private TweenSettings<float> _tweenSettings;
    private IPlayer _player;

    private void Awake()
    {
        _tweenSettings = new TweenSettings<float>();
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
    }

    public void SetPlayer(IPlayer player)
    {
        _player = player ?? throw new ArgumentNullException(nameof(player));
    }
    
    public void ShowUpgrades()
    {
        List<UpgradeWindow> tempWindows = _upgradeWindows.ToList();
        
        Time.timeScale = _pauseTime;
        
        ChangeBackgroundOpacity(_fullVisibility);

        SetWindowsWithWeapons(tempWindows);
        
        SetWindowsWithItems(tempWindows);

        foreach (var upgradeWindow in tempWindows)
        {
            upgradeWindow.ChangeOpacity(_fullVisibility, _changeOpacityTime);

            upgradeWindow.UpgradeSelected += OnUpgradeSelected;
        }
    }

    private void SetWindowsWithItems(List<UpgradeWindow> tempWindows)
    {
        foreach (var upgradeWindow in tempWindows)
        {
            if (upgradeWindow.IsOccupied) 
                continue;
            
            IItem item = _itemsHandler.GetRandomItem();
            
            upgradeWindow.SetWindow(item);
        }
    }

    private void SetWindowsWithWeapons(List<UpgradeWindow> tempWindows)
    {
        for (int i = 0; i < _amountOfWeaponsPerUpgrade; i++)
        {
            Weapon weapon = _itemsHandler.GetRandomWeapon();

            weapon.SetDescription(_player.HasWeapon(weapon) ? weapon.UpgradeDescription : weapon.BaseDescription);
            
            int randomIndex = Random.Range(0, tempWindows.Count);

            tempWindows[randomIndex].SetWindow(weapon);
        }
    }

    private void ChangeBackgroundOpacity(float alphaValue)
    {
        _tweenSettings.endValue = alphaValue;

        Tween.Alpha(_background, _tweenSettings);
    }

    private void OnUpgradeSelected(IItem item)
    {
        ChangeBackgroundOpacity(_fullOpacity);

        foreach (var upgradeWindow in _upgradeWindows)
        {
            upgradeWindow.UpgradeSelected -= OnUpgradeSelected;
            upgradeWindow.ResetSettings();
        }

        Time.timeScale = _normalTimeSpeed;

        UpgradeSelected?.Invoke(item);
    }
}
