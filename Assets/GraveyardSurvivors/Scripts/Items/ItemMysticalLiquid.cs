using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMysticalLiquid : Item, IBuff
{
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue}% to crit multiplier";

    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.CritMultiplier += _increaseValue;
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.CritMultiplier -= _increaseValue;
        
        return baseStats;
    }
}
