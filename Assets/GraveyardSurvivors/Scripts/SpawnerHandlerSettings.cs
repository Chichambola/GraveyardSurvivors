using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpawnerHandlerSettings
{
    public int InitialAvailablePoints;
    public int MaxPoints;
    
    public float MinTime;
    public float MaxTime;
    
    public float SpawnRate;
    public int MaxEnemiesAmount;
    
    public float PointGainPercent;

    public SpawnerHandlerSettings(int availablePoints, int maxPoints, float minTime, float maxTime ,float spawnRate, int maxEnemiesAmount, float pointGainPercent)
    {
        InitialAvailablePoints = availablePoints;
        MaxPoints = maxPoints;
        MinTime = minTime;
        MaxTime = maxTime;
        SpawnRate = spawnRate;
        MaxEnemiesAmount = maxEnemiesAmount;
        PointGainPercent = pointGainPercent;
    }
}