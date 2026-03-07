using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltarHandler : InteractableHandler
{
    [SerializeField] private BloodAltarSpawner _spawner;

    private void OnEnable()
    {
        _spawner.AltarWasChosen += OnAltarChosen;
    }

    private void OnDisable()
    {
        _spawner.AltarWasChosen -= OnAltarChosen;
    }

    private void OnAltarChosen(BloodAltar altar)
    {
        float necessaryHealth = GetAmountOfNecessaryHealth(Player.MaxHealth, altar);

        if (Player.CurrentHealth <= necessaryHealth)
        {
            Debug.Log("Not enough health");
            
            return;
        }

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
