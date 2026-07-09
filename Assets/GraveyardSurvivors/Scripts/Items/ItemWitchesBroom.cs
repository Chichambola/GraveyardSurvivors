using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWitchesBroom : Item, IBuff
{
    [SerializeField] private int _increaseValue;

    public override string CurrentDescription => $"+{_increaseValue} to movement speed";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MovementSpeed = baseStats.MovementSpeed.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MovementSpeed -= baseStats.MovementSpeed.GetClampedValue(_increaseValue);
        
        return baseStats;
    }
}
