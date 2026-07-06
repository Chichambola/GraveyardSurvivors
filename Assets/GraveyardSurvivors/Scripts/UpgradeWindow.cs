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
    [SerializeField] private TextMeshProUGUI _rareLevel;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _desciption;
    
    protected CanvasGroup CanvasGroup;
    protected TweenSettings<float> TweenSettings;
    protected IItem Item;
    
    private float _backgroundAlpha = .3f;
    
    public float Alpha => CanvasGroup.alpha;

    protected virtual void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        TweenSettings = new TweenSettings<float>();
    }

    protected virtual void OnEnable()
    {
        TweenSettings.settings.ease = _easing;
    }

    public void SetWindow(IItem item, ItemSettings settings)
    {
        SetItem(item);

        _rareLevel.text = settings.Rarity != ERarityLevel.None ? settings.Rarity.ToString() : String.Empty;
        
        _background.color = settings.Color;
        
        var backgroundColor = _background.color;
        backgroundColor.a = _backgroundAlpha;
        _background.color = backgroundColor;
    }
    
    public void SetWindow(IItem item) => SetItem(item);

    private void SetItem(IItem item)
    {
        Item = item ?? throw new Exception("Item can not be null");

        _image.overrideSprite = item.Sprite;
        _name.text = item.Name;
        _desciption.text = item.CurrentDescription;
    }

    public Tween ChangeOpacity(float startValue, float endValue, float opacityChangeTime)
    {
        TweenSettings.settings.duration = opacityChangeTime;
        TweenSettings.startValue = startValue;
        TweenSettings.endValue = endValue;
        
        return Tween.Alpha(CanvasGroup, TweenSettings);
    }
}
