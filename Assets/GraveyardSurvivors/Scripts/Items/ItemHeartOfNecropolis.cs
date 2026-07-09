using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHeartOfNecropolis : Item, IBuff
{
    [SerializeField] private int _damageMultiplier = 20;
    [SerializeField] private int _increaseValue;
    
    public override string CurrentDescription => $"+{_increaseValue}% to max health. +{_damageMultiplier}% incoming damage";
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth += _increaseValue;
        
        baseStats.IncomingDamageMultiplier = baseStats.IncomingDamageMultiplier.GetClampedValue(_damageMultiplier);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth -= _increaseValue;
        baseStats.IncomingDamageMultiplier -= baseStats.IncomingDamageMultiplier.GetClampedValue(_damageMultiplier);
        
        return baseStats;
    }
}
