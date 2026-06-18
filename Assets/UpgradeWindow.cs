using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(Button))]
public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private Ease _easing;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _desciption;

    public event Action<IItem> UpgradeSelected;
    
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector3 _initialPosition;
    private TweenSettings<float> _tweenSettings;
    private Button _button;
    private IItem _item;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _button = GetComponent<Button>();
        _tweenSettings = new TweenSettings<float>();
    }

    private void OnEnable()
    {
        _tweenSettings.settings.useUnscaledTime = true;
        _tweenSettings.settings.ease = _easing;
        _canvasGroup.interactable = false;
        _button.interactable = false;
        _initialPosition = _rectTransform.position;
        
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void SetWindow(IItem item)
    {
        _item = item;
        _image.overrideSprite = item.Sprite;
        _name.text = item.Name;
        _desciption.text = item.Description;
    }

    public void ChangeOpacity(float targetOpacity, float opacityChangeTime)
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _button.interactable = true;

        _tweenSettings.settings.duration = opacityChangeTime;
        _tweenSettings.endValue = targetOpacity;
        
        Tween.Alpha(_canvasGroup, _tweenSettings);
    }

    public void Move(float offset, float opacityChangeTime)
    {
        _tweenSettings.settings.duration = opacityChangeTime;
        _tweenSettings.endValue = offset;
        
        Tween.UIAnchoredPositionY(_rectTransform, _tweenSettings);
    }
    
    public void ResetSettings()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
        _rectTransform.anchoredPosition = _initialPosition;
    }

    private void OnButtonClick()
    {
        if (_item == null)
            throw new NullReferenceException("You must assign an item to UpgradeWindow.");
        
        UpgradeSelected?.Invoke(_item);
    }
}
