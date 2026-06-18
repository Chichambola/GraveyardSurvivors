using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemDisplayer : MonoBehaviour
{
    [SerializeField] private float _timeForOpacityChanging;
    [SerializeField] private float _timeBetweenItems;
    [SerializeField] private float _targetOpacityValue;
    [SerializeField] private UpgradeWindow _upgradeWindow;
    
    private Queue<IItem> _itemsToShow;
    private Coroutine _coroutine;
    private float _defaultOpacity = 0;

    private void Awake()
    {
        _itemsToShow = new Queue<IItem>();
    }

    private void OnValidate()
    {
        if (_timeBetweenItems < 0)
        {
            _timeBetweenItems = 0;
        }
        
        if (_timeForOpacityChanging < 0)
        {
            _timeForOpacityChanging = 0;
        }
    }

    private void OnEnable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ShowRoutine());
    }

    public void Enqueue(Item item)
    {
        if (item == null)
        {
            throw new Exception("Item can not be null");
        }
        
        _itemsToShow.Enqueue(item);
    }

    private IEnumerator ShowRoutine()
    {
        var waitForOpacity = new WaitForSecondsRealtime(_timeForOpacityChanging);
        var waitBetweenItems  = new WaitForSecondsRealtime(_timeBetweenItems);
        
        while (enabled)
        {
            if (_itemsToShow.Count <= 0)
            {
                yield return null;
                
                continue;
            }
            
            IItem item = _itemsToShow.Dequeue();

            _upgradeWindow.SetWindow(item);

            _upgradeWindow.ChangeOpacity(_targetOpacityValue, _timeForOpacityChanging);
            
            yield return waitForOpacity;
            yield return waitBetweenItems;

            _upgradeWindow.ChangeOpacity(_defaultOpacity, _timeForOpacityChanging);

            yield return waitForOpacity;
        }
        
        yield return null;
    }
}
