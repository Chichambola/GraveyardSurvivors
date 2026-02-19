using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFeltSocks : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.EvasionChance >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.EvasionChance / HighestValue;

        float multiplier = (1 - clampedValue);
        
        baseStats.EvasionChance = Mathf.Floor(Mathf.Min(baseStats.EvasionChance + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
