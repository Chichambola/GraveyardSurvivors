using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPumpkinSeed : Item, IBuff  
{
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        var newStats = baseStats;
        
        newStats.Health += Mathf.Max(InscreaseValue,0);
        
        return newStats;
    }
}
