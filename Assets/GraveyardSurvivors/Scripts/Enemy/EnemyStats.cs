using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public float Health;
    public float Damage;
    public float Armor;
    public float MovementSpeed;
    public float MoneyForKill;
    public float XpForKill;
    public float LanternEnergy;

    public EnemyStats(EnemyStats stats)
    {
        if (stats == null)
            throw new Exception();
        
        Health = stats.Health;
        Damage = stats.Damage;
        Armor = stats.Armor;
        MovementSpeed = stats.MovementSpeed;
        MoneyForKill = stats.MoneyForKill;
        XpForKill = stats.XpForKill;
        LanternEnergy = stats.LanternEnergy;
    }
}
