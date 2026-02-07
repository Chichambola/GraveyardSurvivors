using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCandy : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;
        
        newStats.HealthRegeneration += Mathf.Max(InscreaseValue, 0);
        
        return newStats;
    }
}
