using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HealAltarHandler : InteractableHandler, IPriceOwner
{
    [SerializeField] private CostHandler _costHandler; 
    
    public void InitializePrices() => InteractableSpawner.SetValueForObjects(_costHandler.Cost);
    
    protected override void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not HealAltar altar)
            throw new Exception(nameof(altar));
        
        if (Player.MoneyAmount < altar.Value)
        {
            Debug.Log("Not enough money");
            
            return;
        }
        
        Player.ReduceMoney(altar.Value);
        
        altar.IncreaseInteractionAmount();
        
        altar.SetValue(_costHandler.IncreaseCost(altar.Value));
    }
}
