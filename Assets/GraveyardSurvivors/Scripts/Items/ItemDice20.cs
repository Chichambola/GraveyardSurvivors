using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDice20 : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.Luck = CalculateBuffAmount(baseStats.Luck);
        
        return baseStats;
    }
}
