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

    [Header("Spawning values")]
    [SerializeField] private int _minTime = 15;
    [SerializeField] private int _maxTime = 60;
    [SerializeField] private float _spawnRate = 0.8f;
    [SerializeField] private int _numberOfSpawnPerSpawner = 3;
    [SerializeField] private int _maxEnemiesAmount = 30;

    public event Action<Enemy> EnemyWasKilled;

    private Coroutine _choosingRoutine;
    private Coroutine _gainRoutine;
    private List<Enemy> _currentEnemies;
    private float _availablePoints;
    private float _debugPoints;
    private int _pointGainMultiplier = 4;

    private void Awake()
    {
        _currentEnemies = new List<Enemy>();
    }

    private void OnEnable()
    {
        _availablePoints = _initialAvailablePoints;

        foreach (IEnemySpawner<Enemy> enemySpawner in _enemySpawners)
        {
            enemySpawner.EnemyWasReleased += OnEnemyRelease;
            enemySpawner.EnemyWasSpawned += _currentEnemies.Add;
        }
    }

    private void OnDisable()
    {
        foreach (IEnemySpawner<Enemy> enemySpawner in _enemySpawners)
        {
            enemySpawner.EnemyWasReleased -= OnEnemyRelease;
            enemySpawner.EnemyWasSpawned -= _currentEnemies.Add;
        }
    }

    private void Start()
    {
        if (_choosingRoutine != null)
            StopCoroutine(_choosingRoutine);

        _choosingRoutine = StartCoroutine(ChoosingRoutine());
    }

    private void OnEnemyRelease(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);
        
        EnemyWasKilled?.Invoke(enemy);
    }

    private IEnumerator ChoosingRoutine()
    {
        var wait = new WaitForSeconds(_spawnRate);
        
        EnemySpawner chosenSpawner = UserUtils.GetElementByWeight(_enemySpawners);

        while (_availablePoints > chosenSpawner.Cost && _currentEnemies.Count < _maxEnemiesAmount)
        {
            _availablePoints -= chosenSpawner.Cost;

            chosenSpawner.Spawn();
            
            chosenSpawner = UserUtils.GetElementByWeight(_enemySpawners);
            
            yield return wait;
        }
        
        StartGainingPoints();

        yield return null;
    }

    private void StartGainingPoints()
    {
        int time = Random.Range(_minTime, _maxTime);

        if (_gainRoutine != null)
            StopCoroutine(_gainRoutine);

        _gainRoutine = StartCoroutine(GainingPointsRoutine(time));
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

        if (_choosingRoutine != null)
            StopCoroutine(_choosingRoutine);

        _choosingRoutine = StartCoroutine(ChoosingRoutine());

        yield return null;
    }
}