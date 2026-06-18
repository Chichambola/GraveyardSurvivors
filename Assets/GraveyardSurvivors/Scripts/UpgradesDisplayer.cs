using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesDisplayer : MonoBehaviour
{
    [Header("Upgrade windows and settings")]
    [SerializeField] private UpgradeWindow[] _upgradeWindows;
    [SerializeField] private float _changeOpacityTime = 0.5f;
    [Header("")]
    [SerializeField] private ItemsHandler _itemsHandler;
    
    public event Action<IItem> UpgradeSelected; 
    
    private float _normalTimeSpeed = 1;
    private float _pauseTime = 0.0001f;
    private float _fullVisibility = 1;
    private float _fullOpacity = 0;
    
    public void ShowUpgrades()
    {
        Time.timeScale = _pauseTime;
        
        foreach (var upgradeWindow in _upgradeWindows)
        {
            IItem item = _itemsHandler.GetRandomItem();
            
            upgradeWindow.SetWindow(item);
            
            upgradeWindow.ChangeOpacity(_fullVisibility, _changeOpacityTime);
            

            upgradeWindow.UpgradeSelected += OnUpgradeSelected;
        }
    }

    private void OnUpgradeSelected(IItem item)
    {
        foreach (var upgradeWindow in _upgradeWindows)
        {
            upgradeWindow.UpgradeSelected -= OnUpgradeSelected;
            upgradeWindow.ResetSettings();
        }
        
        Time.timeScale = _normalTimeSpeed;
        
        UpgradeSelected?.Invoke(item);
    }
}
