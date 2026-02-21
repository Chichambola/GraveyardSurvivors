using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolyWater : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.BlockChance = CalculateBuffAmount(baseStats.BlockChance);
        
        return baseStats;
    }
    
    public override CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.BlockChance -= CalculateBuffAmount(baseStats.BlockChance);
        
        return baseStats;
    }
}
