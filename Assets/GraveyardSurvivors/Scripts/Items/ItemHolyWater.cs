using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolyWater : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;

        newStats.BlockChance = Mathf.Max(InscreaseValue, 0);
        
        return newStats;
    }
}
