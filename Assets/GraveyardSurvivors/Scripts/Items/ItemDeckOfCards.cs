using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeckOfCards : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.AttackSpeed = baseStats.AttackSpeed.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.AttackSpeed -= baseStats.AttackSpeed.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
}
