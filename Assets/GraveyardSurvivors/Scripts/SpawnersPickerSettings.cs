using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct SpawnersPickerSettings
{
    public int InitialAvailablePoints { get; private set; }
    public int MaxPoints { get; private set; }
    public int MaxEnemiesAmount { get; private set; }
    public float SpawnRateMinTime { get; private set; }
    public float SpawnRateMaxTime{ get; private set; }
    public float PointsGainPerSecond { get; private set; }

    public SpawnersPickerSettings(int availablePoints, int maxPoints, float minTime, float maxTime ,float spawnRate, int maxEnemiesAmount, float pointsGainPerSecond, float pointsGainInterval)
    {
        InitialAvailablePoints = availablePoints;
        MaxPoints = maxPoints;
        SpawnRateMinTime = minTime;
        SpawnRateMaxTime = maxTime;
        MaxEnemiesAmount = maxEnemiesAmount;
        PointsGainPerSecond = pointsGainPerSecond;
    }

    public void Upgrade(float percent)
    {
        MaxPoints = MaxPoints.AddPercentToNumber(percent);
        MaxEnemiesAmount = MaxEnemiesAmount.AddPercentToNumber(percent);
    }
}