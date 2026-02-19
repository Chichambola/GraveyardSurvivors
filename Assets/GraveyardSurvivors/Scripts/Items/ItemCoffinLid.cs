using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCoffinLid : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.Armor >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.Armor / HighestValue;

        float multiplier = (1 - clampedValue);
        
        baseStats.Armor = Mathf.Floor(Mathf.Min(baseStats.Armor + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
