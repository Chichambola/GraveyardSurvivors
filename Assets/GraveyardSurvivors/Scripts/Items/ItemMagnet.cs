using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMagnet : Item, IBuff
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;
        
        newStats.PickUpRadius += Mathf.Max(InscreaseValue, 0);
        
        return newStats;
    }
}
