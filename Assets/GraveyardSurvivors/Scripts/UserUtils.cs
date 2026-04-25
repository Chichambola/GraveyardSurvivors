using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class UserUtils
{
    public static readonly float s_HighestPercent = 100;
    public static readonly float s_LowestPercent = 0;

    public static float AddPercentToNumber(this float originalNumber, float percent)
    {
        float finalNumber = originalNumber * (1 + (percent / s_HighestPercent));
        
        return finalNumber;
    }

    public static float SubtractPercentFromNumber(this float originalNumber, float percent)
    {
        float finalNumber = originalNumber *  (1 - percent / s_HighestPercent);

        return finalNumber;
    }

    public static int AddPercentToNumber(this int originalNumber, float percent)
    {
        int finalNumber = Mathf.RoundToInt(originalNumber * (1 + (percent / s_HighestPercent)));
        
        return finalNumber;
    }

    public static float SubtractPercentFromNumber(this int originalNumber, float percent)
    {
        int finalNumber = Mathf.RoundToInt(originalNumber *  (1 - percent / s_HighestPercent));

        return finalNumber;
    }
    
    public static float GetClampedValue(this float originalValue, float increasePercent, float maxThreshold = 100f)
    {
        if (originalValue >= maxThreshold)
            return originalValue;
        
        float leftPercent = maxThreshold - originalValue;
        
        float availablePercent = (leftPercent * increasePercent) / maxThreshold;
        
        originalValue += availablePercent;
        
        return originalValue;
    }
    
    public static T GetElementByWeight<T>(IEnumerable<T> items) where T : IWeightedObject
    {
        T weightedObject = default(T);
        
        float totalWeight = 0;
        
        foreach (var item in items)
        {
            float weight = item.Weight;
            
            if(weight <= 0)
                continue;
            
            float randomNumber = Random.Range(0, totalWeight + weight);

            if (randomNumber >= totalWeight)
            {
                weightedObject = item;
            }
            
            totalWeight += weight;
        }
        
        return weightedObject;

        /*if (items == null)
            throw new Exception(nameof(items));

        float totalWeight = 0;

        var weightedObjects = items.ToList();

        foreach (var item in weightedObjects)
        {
            if (item.Weight > 0)
                totalWeight += item.Weight;
        }

        if (totalWeight <= 0)
            return default;

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var item in weightedObjects)
        {
            if (item.Weight <= 0)
                continue;

            currentWeight += item.Weight;
            if (randomValue < currentWeight)
                return item;
        }

        return default;*/
    }
}
