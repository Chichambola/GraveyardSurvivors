using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJackOLantern : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;

        newStats.AttackRadius += Mathf.Max(InscreaseValue, 0);
        
        return newStats;
    }
}
