using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCoffinLid : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.Armor = CalculateBuffAmount(baseStats.Armor);
        
        return baseStats;
    }
}
