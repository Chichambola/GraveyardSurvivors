using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJackOLantern : Item, IBuff
{
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue}% to attack radius";

    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.AttackRadius = baseStats.AttackRadius.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.AttackRadius -= baseStats.AttackRadius.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
