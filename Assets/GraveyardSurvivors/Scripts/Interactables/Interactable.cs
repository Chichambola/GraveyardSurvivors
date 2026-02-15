using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] protected float Cost;
    [SerializeField] protected Outline Outline;
    
    protected bool IsAvailable = true;

    public bool IsCurrentlyAvailable => IsAvailable;
    
    public float CurrentCost { get; private set; }
    
    private void Awake()
    {
        Outline = GetComponent<Outline>();
    }

    public void ChangeOutlineVisibility(bool value)
    {
        Outline.enabled = value;
    }

    protected void SetCost(float cost)
    {
        CurrentCost = cost;
    }

    public abstract void ProcessInteraction();
}
