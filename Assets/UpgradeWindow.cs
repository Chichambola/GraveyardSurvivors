using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private Ease _easing;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _desciption;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector3 _initialPosition;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup.interactable = false;
        _initialPosition = _rectTransform.position;
    }

    public void SetWindow(IItem item)
    {
        _image.overrideSprite = item.Sprite;
        _name.text = item.Name;
        _desciption.text = item.Description;
    }

    public void ChangeOpacity(float targetOpacity, float opacityChangeTime)
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        Tween.Alpha(_canvasGroup, targetOpacity, opacityChangeTime, _easing);
    }

    public void Move(float offset, float opacityChangeTime)
    {
        Tween.UIAnchoredPositionY(_rectTransform, offset, opacityChangeTime, _easing);
    }
    
    public void ResetSettings()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0;
        _rectTransform.anchoredPosition = _initialPosition;
    }
}
