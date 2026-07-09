using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPumpkinSeed : Item, IBuff
{
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue}% to max health";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth += _increaseValue;
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth -= _increaseValue;
        
        return baseStats;
    }
}
