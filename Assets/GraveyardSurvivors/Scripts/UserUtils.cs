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

    public static int SubtractPercentFromNumber(this int originalNumber, float percent)
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
    
    public static float GetClampedValueInverse(this float originalValue, float increasePercent, float minThreshold = 0f)
    {
        if (originalValue <= minThreshold)
            return originalValue;

        float aboveMin = originalValue - minThreshold;
        
        float availablePercent = (aboveMin * increasePercent) / (100f - minThreshold);
        
        originalValue -= availablePercent;

        return originalValue;
    }
    
    public static T GetElementByWeight<T>(IEnumerable<T> items) where T : IWeightedObject
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));
        
        float totalWeight = 0;
        
        var weightedObjects = items.ToList();
        
        foreach (var item in weightedObjects)
        {
            if (item.Weight > 0)
                totalWeight += item.Weight;
        }
    
        if (totalWeight <= 0)
            return default(T);
        
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

        return default(T);
    }

    public static float RoundToFifths(this float value)
    {
        value = Mathf.Ceil(value * 2) / 2;

        return value;
    }

    public static Vector3 GetRandomOffsetPosition(this Vector3 position, float minValue, float maxValue)
    {
        position.x += Random.Range(minValue, maxValue);
        position.z += Random.Range(minValue, maxValue);

        return position;
    }
}
