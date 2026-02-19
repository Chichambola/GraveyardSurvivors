using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJackOLantern : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.AttackRadius >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.AttackRadius / HighestValue;

        float multiplier = (1 - clampedValue) * (1 - clampedValue);
        
        baseStats.AttackRadius = Mathf.Floor(Mathf.Min(baseStats.AttackRadius + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
