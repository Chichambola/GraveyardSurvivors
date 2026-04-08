using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFeltSocks : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.EvasionChance = baseStats.EvasionChance.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.EvasionChance -= baseStats.EvasionChance.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
}
