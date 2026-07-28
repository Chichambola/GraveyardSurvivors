using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct SpawnersPickerSettings
{
    public int InitialAvailablePoints;
    public int MaxPoints;
    
    public float SpawnRateMinTime;
    public float SpawnRateMaxTime;
    
    public int MaxEnemiesAmount;
    
    [FormerlySerializedAs("PointGainPercent")] public float PointsGainPerSecond;

    public SpawnersPickerSettings(int availablePoints, int maxPoints, float minTime, float maxTime ,float spawnRate, int maxEnemiesAmount, float pointsGainPerSecond, float pointsGainInterval)
    {
        InitialAvailablePoints = availablePoints;
        MaxPoints = maxPoints;
        SpawnRateMinTime = minTime;
        SpawnRateMaxTime = maxTime;
        MaxEnemiesAmount = maxEnemiesAmount;
        PointsGainPerSecond = pointsGainPerSecond;
    }
}