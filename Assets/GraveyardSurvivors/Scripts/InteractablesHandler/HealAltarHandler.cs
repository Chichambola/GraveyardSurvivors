using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HealAltarHandler : InteractableHandler, IPriceOwner
{
    [SerializeField] private CostHandler _costHandler;
    
    private void OnEnable()
    {
        InteractableSpawner.InteractableWasChosen += OnLightAltarChosen;
    }

    private void OnDisable()
    {
        InteractableSpawner.InteractableWasChosen -= OnLightAltarChosen;
    }
    
    private void OnLightAltarChosen(Interactable interactable)
    {
        if (interactable is HealAltar altar == false)
            throw new Exception();

        if (Player.MoneyAmount < altar.Value)
        {
            Debug.Log("Not enough money");
            
            return;
        }
        
        Player.ReduceMoney(altar.Value);
        
        altar.IncreaseInteractionAmount();
        
        altar.SetValue(_costHandler.IncreaseCost(altar.Value));
    }
    
    public void InitializePrices() => InteractableSpawner.SetValueForObjects(_costHandler.Cost);
}
