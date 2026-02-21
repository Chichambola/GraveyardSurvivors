using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPumpkinSeed : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.PickUpRadius = CalculateBuffAmount(baseStats.PickUpRadius);
        
        return baseStats;
    }
}
