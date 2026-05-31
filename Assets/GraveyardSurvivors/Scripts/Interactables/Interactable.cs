using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour, IInteractable, IPoolable<Interactable>
{
    [Header("Interactables specific fields")]
    [SerializeField] protected Outline Outline;
    [SerializeField] protected ValueViewer ValueViewer;
    
    public virtual event Action<Interactable> CanBeReleased;
    public virtual event Action<Interactable> WasChosen;
    
    protected bool IsAvailable = true;
    private bool _isShowingValue;
    
    public bool IsShowingValue => _isShowingValue;
    public bool IsCurrentlyAvailable => IsAvailable;
    public float Value { get; private set; }

    public void SetVisibility(bool value)
    {
        Outline.enabled = value;
        ValueViewer.SetVisibility(value);
        _isShowingValue = value;
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
        
        SetVisibility(false);
        
        WasChosen?.Invoke(this);
    }

    public virtual void Release() => CanBeReleased?.Invoke(this);

    public virtual void ResetCharacteristics() { }
}
