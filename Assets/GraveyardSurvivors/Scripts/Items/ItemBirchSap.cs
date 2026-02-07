using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBirchSap : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;
        
        newStats.AttackSpeed += Mathf.Max(InscreaseValue, 0);
        
        return newStats;
    }
}
