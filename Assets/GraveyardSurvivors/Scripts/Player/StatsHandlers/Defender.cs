using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Defender : Stats<CharacterStats>
{
    [SerializeField] private Player _player;
    [SerializeField] private int _dividerNumber = 2;
    
    private float _armorPercent;
    private float _blockChance;

    protected override void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
    }

    protected override void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
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
        damage = UserUtils.SubstractPercentFromNumber(damage, _armorPercent);

        damage = Mathf.Round(damage);
        
        return damage;
    }
    
    protected override void OnStatsChanged(CharacterStats stats)
    {
        _armorPercent = stats.Armor;
        _blockChance = stats.BlockChance;
    }

    public override void SetInitialStats(CharacterStats stats)
    {
        _armorPercent = stats.Armor;
        _blockChance = stats.BlockChance;
    }
}
