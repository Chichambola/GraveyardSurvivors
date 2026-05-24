using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class CharacterStats
{
    [Header("Health and speed")]
    public float MaxHealth;
    public float HealthRegeneration;
    public float MovementSpeed;
    [Header("Damage")]
    public float AttackSpeed;
    public float AttackRadius;
    public float DamageAmplifier;
    public float IncomingDamageMultiplier;
    [Header("Damage avoidance chances")]
    public float BlockChance;
    public float EvasionChance;
    public float Armor;
    [Header("Crit")]
    public float CritChance;
    public float CritMultiplier;
    [Header("Gold and xp multipliers")]
    public float XpMultiplier;
    public float GoldMultiplier;
    [Header("Misc")]
    public float PickUpRadius;
    public float Luck;
    
    public CharacterStats(CharacterStats stats)
    {
        if (stats == null)
            throw new Exception();
        
        SetStats(stats);
    }
    
    private void SetStats(CharacterStats stats)
    {
        MaxHealth = stats.MaxHealth;
        HealthRegeneration = stats.HealthRegeneration;
        Armor = stats.Armor;
        MovementSpeed = stats.MovementSpeed;
        AttackSpeed = stats.AttackSpeed;
        AttackRadius = stats.AttackRadius;
        DamageAmplifier = stats.DamageAmplifier;
        IncomingDamageMultiplier = stats.IncomingDamageMultiplier;
        PickUpRadius = stats.PickUpRadius;
        BlockChance = stats.BlockChance;
        CritChance = stats.CritChance;
        CritMultiplier = stats.CritMultiplier;
        XpMultiplier = stats.XpMultiplier;
        GoldMultiplier = stats.GoldMultiplier;
        EvasionChance = stats.EvasionChance;
        Luck = stats.Luck;
    }

    public void Upgrade(CharacterStats stats)
    {
        MaxHealth += stats.MaxHealth;
        HealthRegeneration += stats.HealthRegeneration;
        DamageAmplifier += stats.DamageAmplifier; 
        Armor = Armor.GetClampedValue(stats.Armor);
        MovementSpeed = MovementSpeed.GetClampedValue(stats.MovementSpeed);
        AttackSpeed = AttackSpeed.GetClampedValue(stats.AttackSpeed);
        AttackRadius = AttackRadius.GetClampedValue(stats.AttackRadius);
        IncomingDamageMultiplier = IncomingDamageMultiplier.GetClampedValue(stats.IncomingDamageMultiplier);
        PickUpRadius = PickUpRadius.GetClampedValue(stats.PickUpRadius);
        BlockChance = BlockChance.GetClampedValue(stats.BlockChance);
        CritChance = CritChance.GetClampedValue(stats.CritChance);
        CritMultiplier = CritMultiplier.GetClampedValue(stats.CritMultiplier);
        XpMultiplier = XpMultiplier.GetClampedValue(stats.XpMultiplier);
        GoldMultiplier = GoldMultiplier.GetClampedValue(stats.GoldMultiplier);
        EvasionChance = EvasionChance.GetClampedValue(stats.EvasionChance);
        Luck = Luck.GetClampedValue(stats.Luck);
    }
}