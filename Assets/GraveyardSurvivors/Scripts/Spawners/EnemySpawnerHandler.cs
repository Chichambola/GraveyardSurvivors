using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sherbert.Framework.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemySpawnerHandler : MonoBehaviour
{
    [Header("Spawners settings")]
    [SerializeField] private EnemySpawner[] _enemySpawners;
    [SerializeField] private SpawnCollidersHandler _spawnCollidersHandler;
    [SerializeField] private PlacementVerifier _placementVerifier;
    
    [Header("Spawning settings")]
    [SerializeField] private int _minTime = 15;
    [SerializeField] private int _maxTime = 60;
    [SerializeField] private float _spawnRate = 0.8f;
    [SerializeField] private int _maxEnemiesAmount = 30;
    
    [Header("Points settings")]
    [SerializeField] private int _initialAvailablePoints = 30;
    [SerializeField] private int _maxPoints = 250;
    [SerializeField] private float _pointGainPercent = 10;
    
    [Header("Upgrade timing settings")]
    [SerializeField] private SerializableDictionary<int, float> _minutesAndPercentsInitialValues;

    public event Action<Enemy> EnemyWasKilled;

    private Coroutine _choosingRoutine;
    private Coroutine _spawnRoutine;
    private Coroutine _upgradeRoutine;
    private List<Enemy> _currentEnemies;
    private List<EnemySpawner> _availableSpawners;
    private Queue<EnemySpawner> _enemiesToSpawn;
    private Dictionary<int, float> _minutesAndPercents;
    private IntervalTimer _timer;
    private IPlayer _player;
    private float _availablePoints;
    private bool _isChoosing;

    private void Awake()
    {
        _currentEnemies = new List<Enemy>();
        _enemiesToSpawn = new Queue<EnemySpawner>();
        _minutesAndPercents = _minutesAndPercentsInitialValues.ToDictionary(item => item.Key, item => item.Value);
    }

    private void OnEnable()
    {
        _availablePoints = _initialAvailablePoints;
        
        if(_upgradeRoutine != null)
            StopCoroutine(_upgradeRoutine);

        _upgradeRoutine = StartCoroutine(UpgradingRoutine());
    }

    private void OnDisable()
    {
        foreach (var enemySpawner in _availableSpawners)
        {
            enemySpawner.EnemyWasReleased -= OnEnemyRelease;
            enemySpawner.EnemyWasSpawned -= _currentEnemies.Add;
        }
    }
    
    public void Init(IPlayer player)
    {
        if (_enemySpawners.Length <= 0)
            throw new Exception($"Length can not be less than 0");

        _player = player;
        
        FindAvailableSpawners();
        
        StartChoosing();
        
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
        
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private void FindAvailableSpawners()
    {
        foreach (var enemySpawner in _enemySpawners)
        {
            if (!enemySpawner.IsAvailable)
                continue;
            
            enemySpawner.EnemyWasReleased += OnEnemyRelease;
            enemySpawner.EnemyWasSpawned += _currentEnemies.Add;
            
            enemySpawner.SetPlayer(_player);
            
            _availableSpawners.Add(enemySpawner);
        }

        if (_availableSpawners.Count <= 0)
            throw new Exception("You need at lease have 1 available spawner at the beginning.");
    }

    private void StartChoosing()
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

        if (_availablePoints > _maxPoints)
        {
            _availablePoints = _maxPoints;
            
            OnTimerStopped();
        }
    }

    private void OnTimerStopped()
    {
        _timer.Stopped -= OnTimerStopped;
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
                bool canPlace = false;
                
                var spawner = _enemiesToSpawn.Dequeue();

                while (!canPlace)
                {
                    var point = _spawnCollidersHandler.GetRandomPosition();

                    if (!_placementVerifier.IsPlacementValid(point))
                        continue;
                    
                    canPlace = true;
                    
                    spawner.Spawn(point);
                }
                
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
        var wait = new WaitForSecondsRealtime(_spawnRate);
        
        while (_isChoosing && _currentEnemies.Count < _maxEnemiesAmount)
        {
            ChooseSpawner();

            yield return wait;
        }
        
        int time = Random.Range(_minTime, _maxTime);
        
        _timer = new IntervalTimer(time);
        _timer.Stopped += OnTimerStopped;
        _timer.IntervalReached += OnIntervalReached;
        _timer.Start();
    }

    private void ChooseSpawner()
    {
        EnemySpawner chosenSpawner = UserUtils.GetElementByWeight(_availableSpawners);

        if (_availablePoints > chosenSpawner.Cost && _currentEnemies.Count < _maxEnemiesAmount)
        {
            _availablePoints -= chosenSpawner.Cost;

            _enemiesToSpawn.Enqueue(chosenSpawner);
        }
        else
        {
            _isChoosing = false;
        }
    }

    private IEnumerator UpgradingRoutine()
    {
        while (_minutesAndPercents.Count != 0)
        {
            if (_minutesAndPercents.Remove(Game.Minutes, out var percent))
            {
                Upgrade(percent);
            }
            
            yield return null;
        }
    }
    
    private void Upgrade(float percent)
    {
        _spawnRate = _spawnRate.GetClampedValueInverse(percent);
        _pointGainPercent = _pointGainPercent.GetClampedValue(percent);
        
        _maxPoints = _maxPoints.AddPercentToNumber(percent);
        _maxEnemiesAmount = _maxEnemiesAmount.AddPercentToNumber(percent);

        foreach (var enemySpawner in _enemySpawners)
        {
            enemySpawner.Upgrade();
        }
    }
}