using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagnet : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.PickUpRadius = CalculateBuffAmount(baseStats.PickUpRadius);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.PickUpRadius -= CalculateBuffAmount(baseStats.PickUpRadius);
        
        return baseStats;
    }
}
