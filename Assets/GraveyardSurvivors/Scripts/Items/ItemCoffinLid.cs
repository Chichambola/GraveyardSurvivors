using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCoffinLid : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.Armor = baseStats.Armor.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.Armor -= baseStats.Armor.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
}
