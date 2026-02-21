using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCandy : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.HealthRegeneration = CalculateBuffAmount(baseStats.HealthRegeneration);
        
        return baseStats;
    }
}
