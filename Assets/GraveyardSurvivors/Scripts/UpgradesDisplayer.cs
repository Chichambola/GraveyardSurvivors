using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

public class UpgradesDisplayer : MonoBehaviour
{
    [SerializeField] private float _changeOpacityTime = 0.5f;
    [SerializeField] private UpgradeWindow[] _upgradeWindows;
    [SerializeField] private ItemsHandler _itemsHandler;
    [SerializeField] private float _yOffset;


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
            
            upgradeWindow.Move(_yOffset, _changeOpacityTime);
        }
    }
}
