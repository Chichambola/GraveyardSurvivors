using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHeartOfNecropolis : Item
{
    [SerializeField] private int _damageMultiplier = 20;
    
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth += IncreaseValue;
        
        baseStats.IncomingDamageMultiplier = baseStats.IncomingDamageMultiplier.GetClampedValue(_damageMultiplier);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth -= IncreaseValue;
        baseStats.IncomingDamageMultiplier -= baseStats.IncomingDamageMultiplier.GetClampedValue(_damageMultiplier);
        
        return baseStats;
    }
}
