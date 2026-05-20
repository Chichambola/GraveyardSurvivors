using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public float MaxHealth;
    public float Armor;
    public float MovementSpeed;
    public float AttackSpeed;
    public float MoneyForKill;
    public float XpForKill;
    public float LanternEnergy;

    private Dictionary<string, float> _stats;
    
    public EnemyStats(EnemyStats stats)
    {
        if (stats == null)
            throw new Exception();

        SetStats(stats);

        SetDictionary();
    }

    private void SetDictionary()
    {
        _stats = new Dictionary<string, float>()
        {
            {nameof(MaxHealth), MaxHealth},
            {nameof(Armor), Armor},
            {nameof(MovementSpeed), MovementSpeed},
            {nameof(AttackSpeed), AttackSpeed},
            {nameof(MoneyForKill), MoneyForKill},
            {nameof(XpForKill), XpForKill},
            {nameof(LanternEnergy), LanternEnergy},
        };
    }

    private void SetStats(EnemyStats stats)
    {
        MaxHealth = stats.MaxHealth;
        Armor = stats.Armor;
        MovementSpeed = stats.MovementSpeed;
        AttackSpeed = stats.AttackSpeed;
        MoneyForKill = stats.MoneyForKill;
        XpForKill = stats.XpForKill;
        LanternEnergy = stats.LanternEnergy;
    }

    public EnemyStats GetUpgradedStats(EnemyStats currentStats, EnemyStats statsToUpgrade)
    {
        EnemyStats stats = new EnemyStats(currentStats);
        
        foreach (var statsKey in statsToUpgrade._stats.Keys)
        {
            stats._stats[statsKey] += statsToUpgrade._stats[statsKey];
        }
        
        return stats;
    }
}
