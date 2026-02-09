using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDice20 : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;
        
        newStats.Luck = Mathf.Max(InscreaseValue, 0);

        return newStats;
    }
}
