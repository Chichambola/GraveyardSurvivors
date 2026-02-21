using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeckOfCards : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.AttackSpeed = CalculateBuffAmount(baseStats.AttackSpeed);
        
        return baseStats;
    }
}
