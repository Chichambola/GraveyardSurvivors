using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStats : IStat
{
    public float Health;
    public float Armor;
    public float MovementSpeed;
    public float AttackSpeed;
    public float MoneyForKill;
    public float XpForKill;
    public float LanternEnergy;

    public EnemyStats(EnemyStats stats)
    {
        if (stats == null)
            throw new Exception();
        
        Health = stats.Health;
        Armor = stats.Armor;
        MovementSpeed = stats.MovementSpeed;
        AttackSpeed = stats.AttackSpeed;
        MoneyForKill = stats.MoneyForKill;
        XpForKill = stats.XpForKill;
        LanternEnergy = stats.LanternEnergy;
    }
}
