using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMysticalLiquid : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.CritMultiplier = baseStats.PickUpRadius.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.CritMultiplier -= baseStats.PickUpRadius.GetClampedValue(IncreaseValue);
        
        return baseStats;
    }
}
