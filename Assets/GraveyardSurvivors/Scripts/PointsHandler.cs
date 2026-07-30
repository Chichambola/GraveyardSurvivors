using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class PointsHandler : MonoBehaviour
{
    [SerializeField] private float _availablePoints;
    [SerializeField] private float _maxPoints;
    [SerializeField] private float _minTime;
    [SerializeField] private float _maxTime;
    [SerializeField] private float _pointsGainPerSecond;
    
    private CancellationTokenSource _ctsPoints;
    public float AvailablePoints => _availablePoints;

    private void OnValidate()
    {
        if (_availablePoints <= 0)
            _availablePoints = 0;

        if (_availablePoints > _maxPoints)
            _availablePoints = _maxPoints;
    }
    
    public void Upgrade(float percent)
    {
        _maxPoints = _maxPoints.AddPercentToNumber(percent);
    }

    public void ReducePoints(float chosenSpawnerCost)
    {
        _availablePoints -= chosenSpawnerCost;   
    }
    
    public async UniTask GainPoints()
    {
        _ctsPoints = new CancellationTokenSource();
        _ctsPoints.RegisterRaiseCancelOnDestroy(gameObject);

        await GainPointsTask();
            
        _ctsPoints.Cancel();
    }
    
    private async UniTask GainPointsTask()
    {
        float elapsedTime = 0;
        float lastSecond = 0;
        
        float time = Random.Range(_minTime, _maxTime);

        while (!_ctsPoints.IsCancellationRequested && elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float seconds = Mathf.FloorToInt(elapsedTime % 60);
            
            if (!Mathf.Approximately(seconds, lastSecond))
            {
                lastSecond = seconds;
                
                _availablePoints += _pointsGainPerSecond;
            }

            await UniTask.Yield(_ctsPoints.Token);
        }

        if (_availablePoints >= _maxPoints)
        {
            _availablePoints = _maxPoints;
        }
    }
}
