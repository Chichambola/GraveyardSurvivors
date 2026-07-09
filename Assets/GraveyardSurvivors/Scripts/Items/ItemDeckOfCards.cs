using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeckOfCards : Item, IBuff
{
    [SerializeField] private int _increaseValue;
    
    public override string CurrentDescription => $"+{_increaseValue}% to attack speed";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.AttackSpeed = baseStats.AttackSpeed.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.AttackSpeed -= baseStats.AttackSpeed.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
