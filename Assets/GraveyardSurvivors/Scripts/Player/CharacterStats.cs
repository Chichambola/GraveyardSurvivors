using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CharacterStats : IStat
{
    public float Health;
    public float MaxHealth;
    public float HealthRegeneration;
    public float Armor;
    public float MovementSpeed;
    public float AttackSpeed;
    public float AttackRadius;
    public float IncomingDamageMultiplier;
    public float PickUpRadius;
    public float BlockChance;
    public float CritChance;
    public float CritMultiplier;
    public float XpMultiplier;
    public float GoldMultiplier;
    public float EvasionChance;
    public float Luck;
    
    public CharacterStats(CharacterStats stats)
    {
        if (stats == null)
            throw new Exception();
        
        Health = stats.Health;
        MaxHealth = stats.MaxHealth;
        HealthRegeneration = stats.HealthRegeneration;
        Armor = stats.Armor;
        MovementSpeed = stats.MovementSpeed;
        AttackSpeed = stats.AttackSpeed;
        AttackRadius = stats.AttackRadius;
        IncomingDamageMultiplier = stats.IncomingDamageMultiplier;
        PickUpRadius = stats.PickUpRadius;
        BlockChance = stats.BlockChance;
        CritChance = stats.CritChance;
        CritMultiplier = stats.CritMultiplier;
        XpMultiplier = stats.XpMultiplier;
        GoldMultiplier = stats.GoldMultiplier;
        EvasionChance = stats.EvasionChance;
        Luck = stats.Luck;

        if (!Mathf.Approximately(Health, MaxHealth))
        {
            Health = MaxHealth;
        }

        if ((!Mathf.Approximately(MaxHealth, Health)))
        {
            MaxHealth = Health;
        }
    }
}