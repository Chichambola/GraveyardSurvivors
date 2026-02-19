using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolyWater : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.BlockChance >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.BlockChance / HighestValue;

        float multiplier = (1 - clampedValue) * (1 - clampedValue);
        
        baseStats.BlockChance = Mathf.Floor(Mathf.Min(baseStats.BlockChance + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
