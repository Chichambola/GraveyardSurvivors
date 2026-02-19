using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWitchesBroom : Item, IBuff 
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        if (baseStats.MovementSpeed >= HighestValue)
            return baseStats;

        float clampedValue = baseStats.MovementSpeed / HighestValue;

        float multiplier = (1 - clampedValue) * (1 - clampedValue);
        
        baseStats.MovementSpeed = Mathf.Floor(Mathf.Min(baseStats.MovementSpeed + (InscreaseValue * multiplier), HighestValue));
        
        return baseStats;
    }
}
