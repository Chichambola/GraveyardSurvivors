using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJackOLantern : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.AttackRadius = CalculateBuffAmount(baseStats.AttackRadius);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.AttackRadius -= CalculateBuffAmount(baseStats.AttackRadius);
        
        return baseStats;
    }
}
