using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCandy : Item
{
    public override string CurrentDescription => $"+{IncreaseValue}% to health regeneration";

    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.HealthRegeneration = baseStats.HealthRegeneration.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.HealthRegeneration -= baseStats.HealthRegeneration.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
}
