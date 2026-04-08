using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWitchesBroom : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MovementSpeed = baseStats.MovementSpeed.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MovementSpeed -= baseStats.MovementSpeed.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
}
