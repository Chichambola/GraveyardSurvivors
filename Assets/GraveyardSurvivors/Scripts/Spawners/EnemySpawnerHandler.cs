using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemySpawnerHandler : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] _enemySpawners;
    [SerializeField] private int _initialAvailablePoints = 30;

    [Header("Gaining points time")] 
    [SerializeField] private int _minTime = 15;
    [SerializeField] private int _maxTime = 60;
    [SerializeField] private int _maxEnemiesAmount = 30;
    
    public event Action<Enemy> EnemyWasKilled;
    
    private Coroutine _choosingRoutine;
    private Coroutine _gainRoutine;
    private float _availablePoints;
    private float _debugPoints;
    private bool _isGainingPoints;
    private int _pointGainMultiplier = 4;
    
    private void OnEnable()
    {
        _availablePoints = _initialAvailablePoints;
        _isGainingPoints = false;
        
        foreach (IEnemySpawner<Enemy> enemySpawner in _enemySpawners)
        {
            enemySpawner.EnemyWasReleased += OnEnemyRelease;
        }
    }

    private void OnDisable()
    {
        foreach (IEnemySpawner<Enemy> enemySpawner in _enemySpawners)
        {
            enemySpawner.EnemyWasReleased -= OnEnemyRelease;
        }
    }

    private void Start()
    {
        if(_choosingRoutine != null)
            StopCoroutine(_choosingRoutine);
        
        _choosingRoutine = StartCoroutine(ChoosingRoutine());
    }

    private void OnEnemyRelease(Enemy enemy)
    {
        EnemyWasKilled?.Invoke(enemy);
    }

    private IEnumerator ChoosingRoutine()
    {
        while (_isGainingPoints == false)
        {
            int count = GetEnemySpawnersCount();

            Debug.Log(count);
            
            if (count < _maxEnemiesAmount)
            {
                EnemySpawner chosenSpawner = UserUtils.GetElementByWeight(_enemySpawners);
            
                if (_availablePoints > chosenSpawner.Cost)
                {
                    _availablePoints -= chosenSpawner.Cost;
                
                    chosenSpawner.Spawn();
                }
                else if (_availablePoints <= chosenSpawner.Cost)
                {
                    _isGainingPoints = true;

                    int time = Random.Range(_minTime, _maxTime);
                
                    if (_gainRoutine != null)
                        StopCoroutine(_gainRoutine);

                    _gainRoutine = StartCoroutine(GainingPointsRoutine(time));
                }
            }
            
            yield return null;
        }
    }

    private IEnumerator GainingPointsRoutine(float time)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            
            _availablePoints += Time.deltaTime * _pointGainMultiplier;

            yield return null;
        }

        _isGainingPoints = false;
        
        if (_choosingRoutine != null)
            StopCoroutine(_choosingRoutine);

        _choosingRoutine = StartCoroutine(ChoosingRoutine());

        yield return null;
    }

    private int GetEnemySpawnersCount()
    {
        int count = 0;
        
        foreach (var spawner in _enemySpawners)
        {
            count += spawner.Count;
        }

        return count;
    }
}
