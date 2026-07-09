using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFeltSocks : Item, IBuff
{
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue}% to evasion chance";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.EvasionChance = baseStats.EvasionChance.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.EvasionChance -= baseStats.EvasionChance.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
