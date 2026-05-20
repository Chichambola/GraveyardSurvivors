using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = Unity.VisualScripting.ColorUtility;

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

    private Dictionary<string, float> _values;
    
    public EnemyStats(EnemyStats stats)
    {
        if (stats == null)
            throw new Exception();

        SetStats(stats);
    }

    public void SetStats(EnemyStats stats)
    {
        MaxHealth += stats.MaxHealth;
        Armor += stats.Armor;
        MovementSpeed += stats.MovementSpeed;
        AttackSpeed += stats.AttackSpeed;
        MoneyForKill += stats.MoneyForKill;
        XpForKill += stats.XpForKill;
        LanternEnergy += stats.LanternEnergy;
    }
}
