using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Defender : MonoBehaviour
{
    [SerializeField] private int _dividerNumber = 2;

    public bool TryBlockDamage(float blockChance, float luck, ref float damage)
    {
        if (blockChance <= 0)
            return false;
        
        blockChance = blockChance.AddPercentToNumber(luck);

        if (blockChance >= UserUtils.s_HighestPercent)
        {
            return true;
        }

        float randomPercent = Random.Range(UserUtils.s_LowestPercent, UserUtils.s_HighestPercent);
        
        if (randomPercent >= blockChance)
        {
            damage /= _dividerNumber;
            
            return true;
        }

        return false;
    }
    
    public float GetDamageAmount(float armorPercent, float damage)
    {
        damage = damage.SubtractPercentFromNumber(armorPercent);
        
        return damage;
    }
}
