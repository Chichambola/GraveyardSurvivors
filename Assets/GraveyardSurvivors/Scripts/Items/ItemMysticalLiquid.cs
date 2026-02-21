using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMysticalLiquid : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.CritMultiplier = CalculateBuffAmount(baseStats.CritMultiplier);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.CritMultiplier -= CalculateBuffAmount(baseStats.CritMultiplier);
        
        return baseStats;
    }
}
