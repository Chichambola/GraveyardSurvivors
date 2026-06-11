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
    [SerializeField] private Ease _easing;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    private Queue<Item> _itemsToShow;
    private Coroutine _coroutine;
    private float _defaultOpacity = 0;

    private void Awake()
    {
        _itemsToShow = new Queue<Item>();
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
            
            Item item = _itemsToShow.Dequeue();

            SetItem(item);

            ChangeOpacity(_targetOpacityValue);
            
            yield return waitForOpacity;
            yield return waitBetweenItems;

            ChangeOpacity(_defaultOpacity);

            yield return waitForOpacity;
        }
        
        yield return null;
    }

    private void SetItem(Item item)
    {
        _image.overrideSprite = item.Info.Sprite;
            
        _text.text = item.Info.Description;
    }

    private void ChangeOpacity(float targetValue) => Tween.Alpha(_canvasGroup, targetValue, _timeForOpacityChanging, _easing);
}
