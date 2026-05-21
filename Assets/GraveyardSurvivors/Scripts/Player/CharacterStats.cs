using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CharacterStats
{
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
    
    private List<float> _statsToClamp;
    
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
        HealthRegeneration = HealthRegeneration.GetClampedValue(stats.HealthRegeneration);
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