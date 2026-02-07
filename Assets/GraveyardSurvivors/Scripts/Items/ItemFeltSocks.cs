using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFeltSocks : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;

        newStats.EvasionChance += Mathf.Max(InscreaseValue, 0);
        
        return newStats;
    }
}
