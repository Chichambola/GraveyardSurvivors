using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolyWater : Item, IBuff
{
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue}% to block chance";

    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.BlockChance = baseStats.BlockChance.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.BlockChance -= baseStats.BlockChance.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
