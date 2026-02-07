using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCoffinLid : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;
        
        newStats.Armor += Mathf.Max(InscreaseValue, 0);
        
        return newStats;
    }
}
