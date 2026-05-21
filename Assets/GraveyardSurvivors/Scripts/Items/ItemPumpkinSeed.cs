using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPumpkinSeed : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth = baseStats.MaxHealth.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth -= baseStats.MaxHealth.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
}
