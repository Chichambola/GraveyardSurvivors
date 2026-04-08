using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDice20 : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.Luck = baseStats.Luck.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.Luck -= baseStats.Luck.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
}
