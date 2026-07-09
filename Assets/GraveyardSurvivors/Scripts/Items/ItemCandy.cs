using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCandy : Item, IBuff
{
    [SerializeField] private int _increaseValue;
    
    public override string CurrentDescription => $"+{_increaseValue}% to health regeneration";

    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.HealthRegeneration = baseStats.HealthRegeneration.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.HealthRegeneration -= baseStats.HealthRegeneration.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
