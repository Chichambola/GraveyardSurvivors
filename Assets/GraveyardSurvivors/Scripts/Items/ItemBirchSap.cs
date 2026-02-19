using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBirchSap : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.AttackSpeed >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.AttackSpeed / HighestValue;

        float multiplier = (1 - clampedValue) * (1 - clampedValue);
        
        baseStats.AttackSpeed = Mathf.Floor(Mathf.Min(baseStats.AttackSpeed + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
