using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCandy : Item, IBuff
{
    [SerializeField] private int _increaseValue;
    
    public override string CurrentDescription => $"+{_increaseValue}% to health regeneration";

    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var value = baseStats.HealthRegeneration.GetClampedValue(_increaseValue);

        baseStats.HealthRegeneration = baseStats.HealthRegeneration.AddPercentToNumber(value);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        var value = baseStats.HealthRegeneration.GetClampedValueInverse(_increaseValue);
        
        baseStats.HealthRegeneration -= baseStats.HealthRegeneration.SubtractPercentFromNumber(value);
        
        return baseStats;
    }
}
