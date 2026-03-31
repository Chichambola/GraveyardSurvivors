using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable: MonoBehaviour, IInteractable, IPoolable<Interactable>
{
    [Header("Interactables specific fields")]
    [SerializeField] protected Outline Outline;
    [SerializeField] protected ValueViewer ValueViewer;
    
    public virtual event Action<Interactable> CanBeReleased;
    public virtual event Action<Interactable> WasChosen;
    
    protected bool IsAvailable = true;
    protected bool IsShowingValue;
    
    public bool IsCurrentlyShowingValue => IsShowingValue;
    public bool IsCurrentlyAvailable => IsAvailable;
    public float Value { get; private set; }

    public void ChangeOutlineVisibility(bool value)
    {
        Outline.enabled = value;
    }
    
    public void ShowValue()
    {
        ValueViewer.SetVisibility(true);
        IsShowingValue = true;
    }

    public void HideValue()
    {
        ValueViewer.SetVisibility(false);
        IsShowingValue = false;
    }

    public virtual void SetValue(float value)
    {
        Value = value;
        
        ValueViewer.SetValue(value);
    }

    public virtual void ProcessInteraction()
    {
        if (IsAvailable == false)
            return;
        
        HideValue();
        
        WasChosen?.Invoke(this);
    }

    public virtual void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public virtual void ResetCharacteristics()
    {
        
    }
}
