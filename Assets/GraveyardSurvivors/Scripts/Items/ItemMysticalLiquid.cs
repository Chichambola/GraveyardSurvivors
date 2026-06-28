using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMysticalLiquid : Item
{
    public override string CurrentDescription => $"+{IncreaseValue}% to crit multiplier";

    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.CritMultiplier += IncreaseValue;
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.CritMultiplier -= IncreaseValue;
        
        return baseStats;
    }
}
