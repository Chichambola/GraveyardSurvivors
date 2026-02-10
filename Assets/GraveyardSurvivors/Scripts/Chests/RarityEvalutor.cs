using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RarityEvaluator : MonoBehaviour
{
    [SerializeField] private Player _player;

    private int _lowestPercent = 0;
    private int _highestPercent = 100;
    
    public RarityLevel GetRarityLevel(float commonChance, float rareChance, float legendaryChance)
    {
        float currentPercent = GetChance();
        
        if (currentPercent >= _lowestPercent && currentPercent <= commonChance)
        {
            return RarityLevel.Common;
        }

        if (currentPercent > commonChance && currentPercent <= rareChance)
        {
            return RarityLevel.Rare;
        }

        if (currentPercent > legendaryChance && currentPercent <= _highestPercent)
        {
            return RarityLevel.Legendary;
        }
        
        throw new InvalidOperationException("Unable to determine rarity level for the given value");
    }
    
    private float GetChance()
    {
        float percentChance = Random.Range(_lowestPercent, _highestPercent);

        percentChance += _player.CurrentStats.Luck;

        if (percentChance > _highestPercent)
            percentChance = _highestPercent;
        
        return percentChance;
    }
}
