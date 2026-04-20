using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RarityEvaluator : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    public ERarityLevel GetRarityLevel(float commonChance, float rareChance, float legendaryChance)
    {
        rareChance = rareChance.AddPercentToNumber(_player.CurrentStats.Luck);
        legendaryChance = legendaryChance.AddPercentToNumber(_player.CurrentStats.Luck);
        
        var tempRarityDict = new Dictionary<ERarityLevel, float>()
        {
            {ERarityLevel.Common, commonChance},
            {ERarityLevel.Rare, rareChance},
            {ERarityLevel.Legendary, legendaryChance}
        };

        ERarityLevel eRarityLevel = GetLevelByWeight(tempRarityDict);

        return eRarityLevel;
    }
    
    public ERarityLevel GetRarityLevel(float noneChance, float commonChance, float rareChance, float legendaryChance)
    {
        rareChance = rareChance.AddPercentToNumber(_player.CurrentStats.Luck);
        legendaryChance = legendaryChance.AddPercentToNumber(_player.CurrentStats.Luck);
        
        var tempRarityDict = new Dictionary<ERarityLevel, float>()
        {
            {ERarityLevel.None, noneChance},
            {ERarityLevel.Common, commonChance},
            {ERarityLevel.Rare, rareChance},
            {ERarityLevel.Legendary, legendaryChance}
        };

        ERarityLevel eRarityLevel = GetLevelByWeight(tempRarityDict);

        return eRarityLevel;
    }

    private static ERarityLevel GetLevelByWeight(Dictionary<ERarityLevel, float> tempRarityDict)
    {
        ERarityLevel rarityLevel = ERarityLevel.Common;
        
        float totalWeight = 0;

        foreach (var item in tempRarityDict)
        {
            float weight = item.Value;
            
            if(weight <= 0)
                continue;
            
            float randomNumber = Random.Range(0, totalWeight + weight);

            if (randomNumber >= totalWeight)
            {
                rarityLevel = item.Key;
            }
            
            totalWeight += weight;
        }

        return rarityLevel;
    }
}
