using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMysticalLiquid : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.PickUpRadius = CalculateBuffAmount(baseStats.PickUpRadius);
        
        return baseStats;
    }
}
