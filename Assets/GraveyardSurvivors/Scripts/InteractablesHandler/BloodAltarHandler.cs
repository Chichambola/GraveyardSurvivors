using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltarHandler : InteractableHandler
{
    protected override void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not BloodAltar altar)
            throw new Exception(nameof(altar));
        
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
