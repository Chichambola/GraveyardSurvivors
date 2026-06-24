using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeWindowButton : UpgradeWindow
{
    public event Action<IItem> UpgradeSelected;

    private Button _button;
    
    protected override void Awake()
    {
        _button = GetComponent<Button>();
        
        base.Awake();
    }

    protected override void OnEnable()
    {
        _button.interactable = true;
        _button.onClick.AddListener(OnButtonClick);
        
        CanvasGroup.interactable = false;
        TweenSettings.settings.useUnscaledTime = true;
        
        base.OnEnable();
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }
    
    public void SetSettings(bool isInteractable)
    {
        CanvasGroup.interactable = isInteractable;
        CanvasGroup.blocksRaycasts = isInteractable;

        if (!isInteractable)
            CanvasGroup.alpha = 0;
    }
    
    private void OnButtonClick()
    {
        if (Item == null)
            throw new NullReferenceException("You must assign an item to UpgradeWindow.");
        
        UpgradeSelected?.Invoke(Item);
    }
}
