using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJesterCap : Item, IBuff
{
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"{_increaseValue} to crit chance.";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.CritChance = baseStats.CritChance.GetClampedValue(_increaseValue);
        
        return baseStats;
    }

    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.CritChance = baseStats.CritChance.GetClampedValueInverse(_increaseValue);
        
        return baseStats;
    }
}
