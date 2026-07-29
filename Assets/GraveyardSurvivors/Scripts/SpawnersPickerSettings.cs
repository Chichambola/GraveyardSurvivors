using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnersPickerSettings : MonoBehaviour
{
    [SerializeField] private int _availablePoints;
    [SerializeField] private int _maxPoints;
    [SerializeField] private int _maxEnemiesAmount;
    [SerializeField] private float _minTime;
    [SerializeField] private float _maxTime;
    [SerializeField] private float _pointsGainPerSecond;

    public int InitialAvailablePoints => _availablePoints;
    public int MaxPoints => _maxPoints;
    public int MaxEnemiesAmount => _maxEnemiesAmount;
    public float SpawnRateMinTime => _minTime;
    public float SpawnRateMaxTime => _maxTime;
    public float PointsGainPerSecond => _pointsGainPerSecond;
    

    public void Upgrade(float percent)
    {
        MaxPoints = MaxPoints.AddPercentToNumber(percent);
        MaxEnemiesAmount = MaxEnemiesAmount.AddPercentToNumber(percent);
    }
}