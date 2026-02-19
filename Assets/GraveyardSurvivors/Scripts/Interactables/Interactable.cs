using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable: MonoBehaviour, IInteractable
{
    [Header("Interactables specific fields")]
    [SerializeField] protected Outline Outline;
    [SerializeField] protected ValueViewer ValueViewer;
    
    protected bool IsAvailable = true;
    protected bool IsShowingValue = false;
    
    public bool IsCurrentlyShowingValue => IsShowingValue;
    public bool IsCurrentlyAvailable => IsAvailable;
    
    private void Awake()
    {
        Outline = GetComponent<Outline>();
    }

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

    public void SetValue(float value)
    {
        ValueViewer.SetValue(value);
    }
    
    public abstract void ProcessInteraction();
}
