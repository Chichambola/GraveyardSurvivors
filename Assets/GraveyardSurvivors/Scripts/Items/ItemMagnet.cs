using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagnet : Item
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.PickUpRadius >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.PickUpRadius / HighestValue;

        float multiplier = (1 - clampedValue) * (1 - clampedValue);
        
        baseStats.PickUpRadius = Mathf.Floor(Mathf.Min(baseStats.PickUpRadius + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
