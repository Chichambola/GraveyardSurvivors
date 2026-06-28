using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private Ease _easing;
    [SerializeField] private Image _background;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _desciption;
    
    protected CanvasGroup CanvasGroup;
    protected TweenSettings<float> TweenSettings;
    protected IItem Item;
    
    private float _backgroundAlpha = .3f;

    protected virtual void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        TweenSettings = new TweenSettings<float>();
    }

    protected virtual void OnEnable()
    {
        TweenSettings.settings.ease = _easing;
    }

    public void SetWindow(IItem item)
    {
        Item = item ?? throw new Exception("Item can not be null");
        
        _image.overrideSprite = item.Sprite;
        _name.text = item.Name;
        _desciption.text = item.CurrentDescription;
    }

    public void SetWindow(IItem item, Color color)
    {
        SetWindow(item);
        
        _background.color = color;
        
        var backgroundColor = _background.color;
        backgroundColor.a = _backgroundAlpha;
        _background.color = backgroundColor;
    }

    public void ChangeOpacity(float targetOpacity, float opacityChangeTime)
    {
        TweenSettings.settings.duration = opacityChangeTime;
        TweenSettings.endValue = targetOpacity;
        
        Tween.Alpha(CanvasGroup, TweenSettings);
    }
}
