using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDice20 : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.Luck >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.Luck / HighestValue;

        float multiplier = (1 - clampedValue) * (1 - clampedValue);
        
        baseStats.Luck = Mathf.Floor(Mathf.Min(baseStats.Luck + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
