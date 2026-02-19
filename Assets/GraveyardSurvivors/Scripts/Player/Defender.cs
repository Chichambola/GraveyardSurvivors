using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Defender : MonoBehaviour
{
    [SerializeField] private int _dividerNumber = 2;
    
    private float _armorPercent;
    private float _blockChance;
    
    public void SetInitialStats(float armorPercent, float blockChance)
    {
        _armorPercent = armorPercent;
        _blockChance = blockChance;
    }

    public bool CanBlock(float luck)
    {
        float currentBlockChance = _blockChance + luck;

        if (currentBlockChance > UserUtils.HighestPercent)
        {
            return true;
        }

        int randomPercent = Random.Range(UserUtils.LowestPercent, UserUtils.HighestPercent);

        return !(randomPercent > currentBlockChance);
    }

    public float GetBlockedDamage(float damage)
    {
        return Mathf.Round(damage /= _dividerNumber);
    }
    
    public float GetDamageAmount(float damage)
    {
        damage *= (1 - (_armorPercent / UserUtils.HighestPercent));

        damage = Mathf.Round(damage);
        
        Debug.Log($"Damage was blocked! Final blocked damage: {damage}");
        
        return damage;
    }
}
