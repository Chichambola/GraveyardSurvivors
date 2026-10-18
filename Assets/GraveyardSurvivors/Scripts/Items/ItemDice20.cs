using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDice20 : Item, IBuff
{    
    [Header("Luck")]
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue}% to luck";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.Luck = baseStats.Luck.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.Luck -= baseStats.Luck.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
