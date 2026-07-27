using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnersPickerSettings
{
    public int InitialAvailablePoints;
    public int MaxPoints;
    
    public float SpawnRateMinTime;
    public float SpawnRateMaxTime;
    
    public int MaxEnemiesAmount;
    
    public float PointGainPercent;

    public SpawnersPickerSettings(int availablePoints, int maxPoints, float minTime, float maxTime ,float spawnRate, int maxEnemiesAmount, float pointGainPercent, float pointsGainInterval)
    {
        InitialAvailablePoints = availablePoints;
        MaxPoints = maxPoints;
        SpawnRateMinTime = minTime;
        SpawnRateMaxTime = maxTime;
        MaxEnemiesAmount = maxEnemiesAmount;
        PointGainPercent = pointGainPercent;
    }
}