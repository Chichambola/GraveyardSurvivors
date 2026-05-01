using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
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

    [Header("Points gaining")] [SerializeField]
    private int _pointGainPercent = 10;

    public event Action<Enemy> EnemyWasKilled;

    private Coroutine _choosingRoutine;
    private Coroutine _spawnRoutine;
    private List<Enemy> _currentEnemies;
    private List<EnemySpawner> _enemiesToSpawn;
    private IntervalTimer _timer;
    private float _availablePoints;
    private float _debugPoints;
    private bool _isChoosing;

    private void Awake()
    {
        _currentEnemies = new List<Enemy>();
        _enemiesToSpawn = new List<EnemySpawner>();
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
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);

        _spawnRoutine = StartCoroutine(SpawnRoutine());
        
        StartChoosing();
    }

    public void StartChoosing()
    {
        _isChoosing = true;
        
        if (_choosingRoutine != null)
            StopCoroutine(_choosingRoutine);

        _choosingRoutine = StartCoroutine(ChoosingRoutine());
    }

    private void OnEnemyRelease(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);

        EnemyWasKilled?.Invoke(enemy);
    }

    private void OnIntervalReached()
    {
        _availablePoints = _availablePoints.AddPercentToNumber(_pointGainPercent);
    }

    private void OnTimerStopped()
    {
        _timer.TimerStopped -= OnTimerStopped;
        _timer.IntervalReached -= OnIntervalReached;

        _timer?.Stop();
        
        StartChoosing();
    }

    private IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(_spawnRate);

        while (enabled)
        {
            if (_enemiesToSpawn.Count != 0)
            {
                var spawner = _enemiesToSpawn.First();

                spawner.Spawn();

                _enemiesToSpawn.Remove(spawner);

                yield return wait;
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator ChoosingRoutine()
    {
        while (_isChoosing && _currentEnemies.Count < _maxEnemiesAmount)
        {
            ChooseSpawner();

            yield return null;
        }
        
        int time = Random.Range(_minTime, _maxTime);
        
        _timer = new IntervalTimer(time);
        _timer.TimerStopped += OnTimerStopped;
        _timer.IntervalReached += OnIntervalReached;
        _timer.Start();
    }

    private void ChooseSpawner()
    {
        EnemySpawner chosenSpawner = UserUtils.GetElementByWeight(_enemySpawners);

        if (_availablePoints > chosenSpawner.Cost && _currentEnemies.Count < _maxEnemiesAmount)
        {
            _availablePoints -= chosenSpawner.Cost;

            _enemiesToSpawn.Add(chosenSpawner);
        }
        else
        {
            _isChoosing = false;
        }
    }
}