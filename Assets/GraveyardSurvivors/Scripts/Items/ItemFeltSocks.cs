using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFeltSocks : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.EvasionChance = CalculateBuffAmount(baseStats.EvasionChance);
        
        return baseStats;
    }
}
