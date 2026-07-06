using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Sequence = PrimeTween.Sequence;

public class ItemDisplayer : MonoBehaviour
{
    [SerializeField] private float _timeForOpacityChanging;
    [SerializeField] private float _timeBetweenItems;
    [SerializeField] private UpgradeWindow _upgradeWindow;

    private Queue<IItem> _itemsToShow;
    private Coroutine _coroutine;
    private Sequence _opacitySequence;
    private Sequence _sequence;
    private float _fullVisibility = 1;
    private float _fullOpacity = 0;
    private bool _isProcessing;
    private Sequence _currentSequence;

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

    public void Process(Item item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null");

        _itemsToShow.Enqueue(item);

        if (_isProcessing)
            return;
        
        _isProcessing = true;
        
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if (!enabled || _itemsToShow.Count == 0)
        {
            _isProcessing = false;
            
            return;
        }

        IItem item = _itemsToShow.Dequeue();
        
        _upgradeWindow.SetWindow(item);

        _currentSequence = Sequence
            .Create()
            .Chain(_upgradeWindow.ChangeOpacity(_fullOpacity, _fullVisibility, _timeForOpacityChanging))
            .ChainDelay(_timeBetweenItems)
            .Chain(_upgradeWindow.ChangeOpacity(_fullVisibility, _fullOpacity, _timeForOpacityChanging))
            .ChainCallback(ProcessQueue);
    }
}