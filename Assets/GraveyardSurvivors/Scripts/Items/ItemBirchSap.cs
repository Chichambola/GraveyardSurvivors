using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBirchSap : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MovementSpeed = CalculateBuffAmount(baseStats.MovementSpeed);
        
        return baseStats;
    }

    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MovementSpeed -= CalculateBuffAmount(baseStats.MovementSpeed);
        
        return baseStats;
    }
}
