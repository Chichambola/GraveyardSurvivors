using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagnet : Item, IBuff
{
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue}% to pick up radius";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.PickUpRadius = baseStats.PickUpRadius.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.PickUpRadius -= baseStats.PickUpRadius.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
