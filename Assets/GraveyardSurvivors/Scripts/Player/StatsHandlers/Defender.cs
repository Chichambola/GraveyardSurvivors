using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Defender : MonoBehaviour
{
    [SerializeField] private int _dividerNumber = 2;

    public bool CanBlock(float blockChance, float luck)
    {
        blockChance += luck;

        if (blockChance > UserUtils.HighestPercent)
        {
            return true;
        }

        int randomPercent = Random.Range(UserUtils.LowestPercent, UserUtils.HighestPercent);

        return !(randomPercent > blockChance);
    }

    public float GetBlockedDamage(float damage)
    {
        return Mathf.Round(damage /= _dividerNumber);
    }
    
    public float GetDamageAmount(float armorPercent, float damage)
    {
        damage = UserUtils.SubstractPercentFromNumber(damage, armorPercent);

        damage = Mathf.Round(damage);
        
        return damage;
    }
}
