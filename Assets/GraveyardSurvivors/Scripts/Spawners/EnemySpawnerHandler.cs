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
    [SerializeField] private int _choosingRate = 1;

    public event Action<Enemy> EnemyWasKilled;
    
    private Coroutine _choosingRoutine;
    private Coroutine _gainRoutine;
    private EnemySpawner _chosenSpawner;
    private float _availablePoints;
    private int _pointGainMultiplier = 4;
    private bool _isGainingPoints;
    private float _debugPoints;
    
    private void OnEnable()
    {
        _availablePoints = _initialAvailablePoints;
        _isGainingPoints = false;
        
        foreach (IEnemySpawner<Enemy> enemySpawner in _enemySpawners)
        {
            enemySpawner.EnemyWasReleased += OnEnemyRelease;
        }
        
        if(_choosingRoutine != null)
            StopCoroutine(_choosingRoutine);
        
        _choosingRoutine = StartCoroutine(ChoosingRoutine());
    }

    private void OnDisable()
    {
        foreach (IEnemySpawner<Enemy> enemySpawner in _enemySpawners)
        {
            enemySpawner.EnemyWasReleased -= OnEnemyRelease;
        }
    }

    private void OnEnemyRelease(Enemy enemy)
    {
        EnemyWasKilled?.Invoke(enemy);
    }

    private IEnumerator ChoosingRoutine()
    {
        while (_isGainingPoints == false)
        {
            EnemySpawner chosenSpawner = UserUtils.GetElementByWeight(_enemySpawners);

            if (_availablePoints > chosenSpawner.Cost)
            {
                _availablePoints -= chosenSpawner.Cost;
                
                chosenSpawner.Spawn();
            }
            
            yield return null;
        }
    }

    private IEnumerator GainingPointsRoutine(float time)
    {
        _isGainingPoints = true;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            
            _availablePoints += Time.deltaTime * _pointGainMultiplier;

            yield return null;
        }
    }
}
