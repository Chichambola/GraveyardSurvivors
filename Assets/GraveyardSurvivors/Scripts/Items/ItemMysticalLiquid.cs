using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMysticalLiquid : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;
        
        newStats.CritMultiplier += Mathf.Max(InscreaseValue, 0);
        
        return newStats;
    }
}
