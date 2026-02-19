using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMysticalLiquid : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.CritMultiplier >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.CritMultiplier / HighestValue;

        float multiplier = (1 - clampedValue) * (1 - clampedValue);
        
        baseStats.CritMultiplier = Mathf.Floor(Mathf.Min(baseStats.CritMultiplier + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
