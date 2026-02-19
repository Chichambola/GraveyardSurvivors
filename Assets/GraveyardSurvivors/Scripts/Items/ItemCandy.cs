using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCandy : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.HealthRegeneration >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.HealthRegeneration / HighestValue;

        float multiplier = (1 - clampedValue) * (1 - clampedValue);
        
        baseStats.HealthRegeneration = Mathf.Floor(Mathf.Min(baseStats.HealthRegeneration + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
