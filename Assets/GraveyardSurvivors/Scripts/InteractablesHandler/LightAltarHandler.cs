using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LightAltarHandler : CostInteractableHandler
{
    private void OnEnable()
    {
        InteractableSpawner.InteractableWasChosen += OnLightAltarChosen;
    }

    private void OnDisable()
    {
        InteractableSpawner.InteractableWasChosen -= OnLightAltarChosen;
    }

    private void Start()
    {
        InteractableSpawner.SetValueForObjects(Cost);
    }
    
    private void OnLightAltarChosen(Interactable interactable)
    {
        if (interactable is LightAltar altar == false)
            throw new Exception();

        if (Player.MoneyAmount < altar.Value)
        {
            Debug.Log("Not enough money");
            
            return;
        }
        
        Player.ReduceMoney(altar.Value);
        
        altar.IncreaseInteractionAmount();
        
        altar.SetValue(IncreaseCost(altar.Value));
    }
}
