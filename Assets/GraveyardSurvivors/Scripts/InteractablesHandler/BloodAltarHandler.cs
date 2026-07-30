using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltarHandler : InteractableHandler
{
    private void OnEnable()
    {
        InteractableSpawner.InteractableWasChosen += OnAltarChosen;
    }

    private void OnDisable()
    {
        InteractableSpawner.InteractableWasChosen -= OnAltarChosen;
    }
    
    private void OnAltarChosen(Interactable interactable)
    {
        if (interactable is BloodAltar altar == false)
            throw new Exception();
        
        float necessaryHealth = GetAmountOfNecessaryHealth(Player.MaxHealth, altar);

        if (Player.CurrentHealth <= necessaryHealth)
        {
            Debug.Log("Not enough health");
            
            return;
        }

        altar.StartCountdown();
        
        altar.IncreaseInteractionAmount();

        float moneyAmount = Mathf.Round(necessaryHealth / 2);
        
        Player.TakeDamage(necessaryHealth);
        Player.ReceiveMoney(moneyAmount);
    }
    
    private float GetAmountOfNecessaryHealth(float maxPlayerHealth, BloodAltar altar)
    {
        float maxPercent = 100;
        
        float damagePercent = altar.GetDamagePercent();
        
        float damage = maxPlayerHealth * (damagePercent / maxPercent);
        
        return damage;
    }
}
