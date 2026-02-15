using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RarityEvaluator : MonoBehaviour
{
    [SerializeField] private Player _player;

    private int _lowestPercent = 0;
    private int _highestPercent = 100;
    
    public ERarityLevel GetRarityLevel(float commonChance, float rareChance, float legendaryChance)
    {
        float currentPercent = GetChance();
        
        var ranges = new[]
        {
            (max: _lowestPercent, rarity: ERarityLevel.Common),
            (max: commonChance, rarity: ERarityLevel.Common),
            (max: rareChance, rarity: ERarityLevel.Rare),
            (max: legendaryChance, rarity: ERarityLevel.Legendary),
            (max: _highestPercent, rarity: ERarityLevel.Legendary)
        };
        
        var rarityLevel = ranges.First(r => currentPercent < r.max).rarity;
        
        return rarityLevel;
    }
    
    public ERarityLevel GetRarityLevel(float commonChance, float rareChance, float legendaryChance, out float currentPercent)
    {
        currentPercent = GetChance();
        
        var ranges = new[]
        {
            (max: _lowestPercent, rarity: ERarityLevel.Common),
            (max: commonChance, rarity: ERarityLevel.Common),
            (max: rareChance, rarity: ERarityLevel.Rare),
            (max: legendaryChance, rarity: ERarityLevel.Legendary),
            (max: _highestPercent, rarity: ERarityLevel.Legendary)
        };

        var currentNumber = currentPercent;
        
        var rarityLevel = ranges.First(r => currentNumber < r.max).rarity;
        
        return rarityLevel;
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
