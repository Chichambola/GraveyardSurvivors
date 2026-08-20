using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

public class HealAltarHandler : InteractableHandler, IPriceOwner
{
    [SerializeField] private CostHandler _costHandler;

    private static int _altarsWithPlayerAroundCount;
    
    public void InitializePrices() => InteractableSpawner.SetValueForObjects(_costHandler.Cost);

    public static void IncreaseCount()
    {
        _altarsWithPlayerAroundCount++;
    }
    
    public static void DecreaseCount()
    {
        _altarsWithPlayerAroundCount--;
    }

    public static bool CanStartLight() => _altarsWithPlayerAroundCount <= 0;
    
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
