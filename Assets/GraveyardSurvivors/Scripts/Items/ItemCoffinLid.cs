using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCoffinLid : Item, IBuff
{    
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue}% to armor";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.Armor = baseStats.Armor.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.Armor -= baseStats.Armor.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
